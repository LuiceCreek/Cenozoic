using System;
using System.Net;
using System.Text;
using System.Threading;

namespace Cenozoic
{
    class Program
    {
        private static readonly int reloadInterval = 30000;
        private static readonly string publicTimelineUrl = "https://mstdn-workers.com/api/v1/timelines/public?local=1";
        private static int postLastId = 0;

        static void Main(string[] args)
        {
            Timer timer = new Timer(new TimerCallback(ThreadingTimerCallback));
            timer.Change(0, Timeout.Infinite);
            Console.ReadLine();
        }

        private static void ThreadingTimerCallback(object state)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                string publicTimeline = client.DownloadString(publicTimelineUrl);
                Console.WriteLine(publicTimeline);
            }

            Timer timer = state as Timer;
            timer.Change(reloadInterval, Timeout.Infinite);
        }
    }
}
