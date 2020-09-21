using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ParallelUniverse.WebApi.PuAuth
{
    public class MyUniverseAuthHandler : AuthenticationHandler<MyUniverseAuthOptions>
    {
        private readonly UserService _userSvc;

        public MyUniverseAuthHandler(
            UserService userSvc,
            IOptionsMonitor<MyUniverseAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _userSvc = userSvc;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authStrValue))
            {
                return AuthenticateResult.NoResult();
            }

            if(!AuthenticationHeaderValue.TryParse(authStrValue, out var authHeaderValue))
            {
                return AuthenticateResult.NoResult();
            }

            if(!"Bearer".Equals(authHeaderValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            var user = _userSvc.GetSession(authHeaderValue.Parameter);
            
            if(user == null)
            {
                return AuthenticateResult.Fail("Invalid token");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name)
            };

            var identity = new ClaimsIdentity(claims, "UniverseAuth");
            // identity.IsAuthenticated must be true, otherwise it is still an unauthenticated user.
            var principal = new ClaimsPrincipal(identity);
            var authTicket = new AuthenticationTicket(principal, "UniverseAuth");

            await Task.CompletedTask;
            return AuthenticateResult.Success(authTicket);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            return base.HandleChallengeAsync(properties);
        }
    }
}
