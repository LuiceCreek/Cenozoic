using Codeplex.Data;
using FNF.Utility;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Cenozoic
{
    class Program
    {
        private static BouyomiChanClient BouyomiChan;
        private static readonly string BouyomiConnectedMessage = "読み上げを開始します。";
        private static readonly string connectedErrorMessage = "棒読みちゃんに接続できませんでした。";
        private static readonly string sensitiveWarningMessage = "不適切なコンテンツです。";
        private static readonly string publicTimelineBaseUrl = "https://mstdn-workers.com/api/v1/timelines/public";
        private static readonly int reloadInterval = 30000;
        private static ulong postLastId = 0;

        static void Main(string[] args)
        {
            try
            {
                BouyomiChan = new BouyomiChanClient();
                BouyomiChan.AddTalkTask(BouyomiConnectedMessage);
                Timer timer = new Timer(new TimerCallback(ThreadingTimerCallback));
                timer.Change(0, Timeout.Infinite);
            }
            catch
            {
                Console.WriteLine(connectedErrorMessage);
            }

            Console.ReadLine();
            BouyomiChan.Dispose();
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
                foreach (dynamic status in Enumerable.Reverse((object[])statuses))
                {
                    string speechMessage = TagRemover(status.content);
                    if (status.sensitive == true)
                    {
                        speechMessage = sensitiveWarningMessage;
                    }
                    BouyomiChan.AddTalkTask(speechMessage);
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
