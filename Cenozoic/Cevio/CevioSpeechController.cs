using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CeVIO.Talk.RemoteService;

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
            SpeakingState state = talk.Speak(trim(text));
            state.Wait();
        }

        public void Terminate()
        {
            ServiceControl.CloseHost();
        }

        private string trim(string text)
        {
            if (text.Length < 100)
            {
                return text;
            }
            return text.Substring(1, 95) + "、以下略";
        }
    }
}
