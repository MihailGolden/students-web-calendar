using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using WebCalendar.Models;

namespace WebCalendar.Hubs
{
    public class NotifyTime
    {
        internal readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        private static List<Notify> dates = new List<Notify>();
        private object updateLock = new object();
        private readonly System.Timers.Timer timer = new System.Timers.Timer();
        private readonly static Lazy<NotifyTime> instance =
        new Lazy<NotifyTime>(() => new NotifyTime());
        private IHubContext context;
        private Notify time;

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
                    int result = DateTime.Compare(time.Date, date);

                    if (result == 0)
                    {
                        dates.Remove(time);
                        foreach (var connectionId in _connections.GetConnections(_connections.UserID))
                        {
                            context.Clients.Client(connectionId).send(time.Title,time.Date);
                        }
                        //context.Clients.All.send(time);
                        Thread.Sleep(Helpers.Constants.MilliSeconds_Timeout);
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
        public void GetDates(List<Notify> list)
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
                return DateTime.Now.ToString(Helpers.Constants.String_Format);
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

        public override Task OnConnected()
        {
            var user = Context.User;

            if (user.Identity.IsAuthenticated)
            {
                string userId = Context.User.Identity.GetUserId();
                NotifyTime._connections.UserID = userId;
                NotifyTime._connections.Add(userId, Context.ConnectionId);
                //string name = Context.User.Identity.Name;
                //Groups.Add(Context.ConnectionId, name);
            }
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            var user = Context.User;

            if (user.Identity.IsAuthenticated)
            {
                string userId = user.Identity.GetUserId();

                if (!NotifyTime._connections.GetConnections(userId).Contains(Context.ConnectionId))
                {
                    NotifyTime._connections.Add(userId, Context.ConnectionId);
                }
            }
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var user = Context.User;

            if (user.Identity.IsAuthenticated)
            {
                string userId = user.Identity.GetUserId();
                NotifyTime._connections.Remove(userId, Context.ConnectionId);
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}