using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Eyedia.Aarbac.Api.Models;
using Eyedia.Aarbac.Api.Service;
using Eyedia.Aarbac.Framework;

namespace Eyedia.Aarbac.Api.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string rbacName = context.OwinContext.Get<string>("rbac");
            if (string.IsNullOrEmpty(rbacName))
                RbacException.Raise("Parameter rbac was not passed in the request!", RbacExceptionCategories.Web);

            Rbac rbac = new Rbac(context.UserName);
            //RbacUser user = rbac.Authenticate(context.UserName, context.Password);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, rbac.User.UserName));
            claims.Add(new Claim(ClaimTypes.Email, rbac.User.Email));
            claims.Add(new Claim("rbac", rbacName));
            var oAuthIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            Dictionary<string, string> kvPair = new Dictionary<string, string>();
            kvPair.Add("rbacid", rbac.RbacId.ToString());
            kvPair.Add("fullname", rbac.User.FullName);
            var props = new AuthenticationProperties(kvPair);
            

            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, props);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(oAuthIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {            
            string rbac = context.Parameters.Where(i => i.Key.ToLower() == "rbac").Select(i => i.Value).SingleOrDefault()[0];
            context.OwinContext.Set<string>("rbac", rbac);

            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                //if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}