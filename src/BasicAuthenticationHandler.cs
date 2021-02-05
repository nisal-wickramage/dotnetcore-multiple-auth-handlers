using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace multiauth
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
    }

    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Console.WriteLine("BasicAuthenticationHandler");
            
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Unauthorized");
 
            string apiKey = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
 
            if (apiKey.Equals("basic"))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "test"),
                };
 
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new System.Security.Principal.GenericPrincipal(identity, null);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Unauthorized");
        }
    }
}
