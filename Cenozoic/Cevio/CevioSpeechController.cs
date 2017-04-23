using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CeVIO.Talk.RemoteService;
using System.Text.RegularExpressions;

namespace Cenozoic.Cevio
{
    public class CevioSpeechController : ISpeechController
    {
        private Talker talk;

        public void Initialize()
        {
            ServiceControl.StartHost(false);
            talk = new Talker();
            talk.Cast = "さとうささら";
            talk.Volume = 100;
        }

        public void Speak(string text)
        {
            string msg = transmitter(text);
            if (String.IsNullOrEmpty(msg)) return;

            SpeakingState state = talk.Speak(msg);
            state.Wait();
        }

        public void Terminate()
        {
            ServiceControl.CloseHost();
        }

        private string transmitter(string text)
        {
            string msg = replaceUrlToEmpty(text);
            // 文字数が100文字を超えると、Cevioがワーニングを出す為
            if (msg.Length < 100) return msg;
            return msg.Substring(1, 95) + "、以下略";
        }

        private string replaceUrlToEmpty(string text)
        {
            Regex regex = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return regex.Replace(text, string.Empty);
        }
    }
}
