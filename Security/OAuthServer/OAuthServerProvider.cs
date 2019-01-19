using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Security.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Security.OAuthServer
{
    public class OAuthServerProvider : OAuthAuthorizationServerProvider
    {
        private const string _specialPermissionPrefix = "sp_";
        private const string _rolePrefix = "r_";
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //string clId, clSecret;
            //if (context.TryGetFormCredentials(out clId, out clSecret))
            //{
            //    // validate the client Id and secret against database or from configuration file.  
            //    context.Validated();
            //}
            //else if (context.TryGetBasicCredentials(out clId, out clSecret))
            //{
            //    // validate the client Id and secret against database or from configuration file.  
            //    context.Validated();
            //}
            //else
            //{
            //    context.SetError("invalid_client", "Client credentials could not be retrieved from the Authorization header");
            //    context.Rejected();
            //}
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            UserManager<User> userManager = context.OwinContext.GetUserManager<UserManager<User>>();
            User user;
            try
            {
                user = await userManager.FindAsync(context.UserName, context.Password);
            }
            catch
            {
                // Could not retrieve the user due to error.  
                context.SetError("server_error");
                context.Rejected();
                return;
            }
            if (user != null)
            {
                ClaimsIdentity identity = await userManager.CreateIdentityAsync(
                                                        user,
                                                        DefaultAuthenticationTypes.ExternalBearer);
                
                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { "username", user.UserName },
                });
                var userRoles = await userManager.GetRolesAsync(user.Id);
                foreach (var item in userRoles)
                {
                    props.Dictionary.Add(_rolePrefix + item, "true");
                }

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            else
            {
                context.SetError("Authentication failed", "Invalid User Id or password'");
                context.Rejected();
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization" });
                context.RequestCompleted();
                return Task.FromResult(0);
            }

            return base.MatchEndpoint(context);
        }
    }
}