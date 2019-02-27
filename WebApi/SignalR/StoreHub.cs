using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebApi.SignalR
{
    public class StoreHub: Hub
    {
        private static IHubContext _hubContext = GlobalHost.ConnectionManager.GetHubContext<StoreHub>();

        public void Hello()
        {
            Clients.All.hello();
        }

        public static void SayHello()
        {
            _hubContext.Clients.All.hello();
        }
    }
}