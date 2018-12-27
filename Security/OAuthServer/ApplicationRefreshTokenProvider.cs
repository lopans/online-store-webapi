using Base.Exceptions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.OAuthServer
{
    public class ApplicationRefreshTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            // Expiration time in seconds
            int expire = 5 * 60;
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddSeconds(expire));
            context.SetToken(context.SerializeTicket());
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            try
            {
                context.DeserializeTicket(context.Token);
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            }
            catch(Exception ex)
            {

            }
        }
    }
}
