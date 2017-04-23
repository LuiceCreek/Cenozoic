using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cenozoic
{
    interface ISpeechController
    {
        void Initialize();

        void Speak(string text);

        void Terminate();
    }
}
