using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using ParallelUniverse.Model;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParallelUniverse.Spa
{
    public class UniverseAuthStateProvider : AuthenticationStateProvider
    {
        private const string AuthTokenName = "AuthToken";
        private readonly HttpClient _httpClient;
        private readonly ISessionStorageService _sessionStorage;
        private TaskCompletionSource<AuthenticationState> _tcsAuthState;

        public UniverseAuthStateProvider(HttpClient httpClient, ISessionStorageService sessionStorage)
        {
            _httpClient = httpClient;
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_tcsAuthState != null)
            {
                return await _tcsAuthState.Task;
            }

            var token = await _sessionStorage.GetItemAsync<string>(AuthTokenName);

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("me");

            if (response.IsSuccessStatusCode)
            {
                var userName = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(userName))
                {
                    var authState = new AuthenticationState(
                        new ClaimsPrincipal(
                            new ClaimsIdentity(
                                claims: new[] { new Claim(ClaimTypes.Name, userName) },
                                authenticationType: "UniverseAuthentication")));
                    _tcsAuthState = new TaskCompletionSource<AuthenticationState>();
                    _tcsAuthState.TrySetResult(authState);

                    return authState;
                }
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task<bool> LoginAsync(LoginInfo loginInfo)
        {
            var response = await _httpClient.PostAsJsonAsync("login", loginInfo);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    await _sessionStorage.SetItemAsync(AuthTokenName, token);

                    var authState = new AuthenticationState(
                        new ClaimsPrincipal(
                            new ClaimsIdentity(
                                claims: new[] { new Claim(ClaimTypes.Name, loginInfo.UserName) },
                                authenticationType: "UniverseAuthentication")));

                    _tcsAuthState = new TaskCompletionSource<AuthenticationState>();
                    _tcsAuthState.SetResult(authState);

                    NotifyAuthenticationStateChanged(_tcsAuthState.Task);
                    return true;
                }
            }

            return false;
        }

        public Task<string> GetTokenAsync()
        {
            return _sessionStorage.GetItemAsync<string>(AuthTokenName);
        }
    }
}
