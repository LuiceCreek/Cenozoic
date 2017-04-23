using FNF.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cenozoic.Bouyomi
{
    class BouyomiController : ISpeechController
    {
        private BouyomiChanClient bouyomiChan;

        public void Initialize()
        {
            bouyomiChan = new BouyomiChanClient();
        }

        public void Speak(string text)
        {
            bouyomiChan.AddTalkTask(text);
        }

        public void Terminate()
        {
            bouyomiChan.Dispose();
        }
    }
}
