using Codeplex.Data;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Cenozoic
{
    class Program
    {
        private static readonly int reloadInterval = 30000;
        private static readonly string publicTimelineBaseUrl = "https://mstdn-workers.com/api/v1/timelines/public";
        private static ulong postLastId = 0;

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
                string publicTimelineUrl = String.Format("{0}?local={1}&since_id={2}", publicTimelineBaseUrl, true, postLastId);

                client.Encoding = Encoding.UTF8;
                string publicTimeline = client.DownloadString(publicTimelineUrl);
                dynamic statuses = DynamicJson.Parse(publicTimeline);

                postLastId = (ulong)statuses[0]["id"];
                foreach (dynamic status in statuses)
                {
                    Console.WriteLine(TagRemover(status.content));
                }
            }

            Timer timer = state as Timer;
            timer.Change(reloadInterval, Timeout.Infinite);
        }

        private static string TagRemover(string content)
        {
            return Regex.Replace(content, "<.*?>", string.Empty);
        }
    }
}
