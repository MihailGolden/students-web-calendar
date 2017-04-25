using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using WebCalendar.Helpers;
namespace WebCalendar.Hubs
{
    public class NotifyTime
    {
        private static List<DateTime> dates = new List<DateTime>();
        private object updateLock = new object();
        private readonly System.Timers.Timer timer = new System.Timers.Timer();
        private readonly static Lazy<NotifyTime> instance =
        new Lazy<NotifyTime>(() => new NotifyTime());
        private IHubContext context;
        private DateTime time;

        private NotifyTime()
        {
            this.context = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
            timer.Interval = 1000;
            timer.Elapsed += _Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
        }
        private void _Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(updateLock))
            {
                try
                {
                    var date = DateTime.Parse(CurrentTime);
                    date.AddSeconds(-date.Second).AddMilliseconds(-date.Millisecond);
                    int result = DateTime.Compare(time, date);

                    if (result == 0)
                    {
                        dates.Remove(time);
                        context.Clients.All.send(time);
                        Thread.Sleep(Constants.MilliSeconds_Timeout);
                        if (dates.Count > 0)
                        {
                            time = dates[0];
                        }
                    }
                    else if (result < 0)
                    {
                        dates.Remove(time);
                        if (dates.Count > 0)
                        {
                            time = dates[0];
                        }
                    }
                }
                finally
                {
                    Monitor.Exit(updateLock);
                }
            }
        }
        public void GetDates(List<DateTime> list)
        {
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    dates.Add(item);
                }
                time = dates[0];
            }
        }

        public static NotifyTime Instance
        {
            get
            {
                return instance.Value;
            }
        }
        public string CurrentTime
        {
            get
            {
                return DateTime.Now.ToString(Constants.String_Format);
            }
        }
    }
    public class NotifyHub : Hub
    {
        private readonly NotifyTime notify;

        public NotifyHub() : this(NotifyTime.Instance) { }

        public NotifyHub(NotifyTime notify)
        {
            this.notify = notify;
        }

        public string GetInitialTime()
        {
            return notify.CurrentTime;
        }
    }
}