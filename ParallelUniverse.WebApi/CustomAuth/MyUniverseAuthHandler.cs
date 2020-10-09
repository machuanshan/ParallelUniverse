using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
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
            Logger.LogInformation($"Authorizing, method: {Request.Method}, url: {Request.GetDisplayUrl()}");

            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authStrValue))
            {
                Logger.LogWarning($"No authorization header found");
                return AuthenticateResult.NoResult();
            }

            if (!AuthenticationHeaderValue.TryParse(authStrValue, out var authHeaderValue))
            {
                Logger.LogWarning($"Failed to parse authorization header: {authHeaderValue}");
                return AuthenticateResult.NoResult();
            }

            if (!Constants.BearerScheme.Equals(authHeaderValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                Logger.LogWarning($"Unsupported authentication scheme: {authHeaderValue.Scheme}");
                return AuthenticateResult.NoResult();
            }

            var user = _userSvc.GetSession(authHeaderValue.Parameter);

            if (user == null)
            {
                Logger.LogWarning($"Session not found for token: {authHeaderValue.Parameter}");
                return AuthenticateResult.Fail("Invalid token");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name)
            };

            var identity = new ClaimsIdentity(claims, Constants.MyUniverseScheme);
            // identity.IsAuthenticated must be true, otherwise it is still an unauthenticated user.
            var principal = new ClaimsPrincipal(identity);
            var authTicket = new AuthenticationTicket(principal, Constants.MyUniverseScheme);

            await Task.CompletedTask;
            Logger.LogInformation($"Authentication succeeded, user: {user.Name}");
            return AuthenticateResult.Success(authTicket);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Logger.LogWarning($"Challenge called, url: {Request.GetDisplayUrl()}");
            return base.HandleChallengeAsync(properties);
        }
    }
}
