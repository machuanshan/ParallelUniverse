using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ParallelUniverse.Model;
using ParallelUniverse.WebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParallelUniverse.WebApi
{
    public class UserService
    {
        private readonly IConfiguration _configuration;
        private IMemoryCache _sessions;
        private readonly ILogger<UserService> _logger;
        private readonly AppData _appData;

        public UserService(IConfiguration configuration, IMemoryCache memoryCache, ILogger<UserService> logger)
        {
            _configuration = configuration;
            var jsonTxt = File.ReadAllText("./appdata.json");
            _appData = JsonSerializer.Deserialize<AppData>(jsonTxt);
            _sessions = memoryCache;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string Login(LoginInfo loginInfo)
        {
            var userInfo = _appData.Users.FirstOrDefault(
                u => u.Name == loginInfo.UserName && u.Password == loginInfo.Password);

            if(userInfo != null)
            {
                var token = Utils.GetRandomString(32);
                _sessions.Set(token, userInfo, TimeSpan.FromMinutes(10));
                _logger.LogInformation($"Login for user: {loginInfo.UserName}");

                return token;
            }

            _logger.LogInformation($"User not found: {loginInfo.UserName}");
            return null;
        }

        public UserInfo GetSession(string sessionToken)
        {
            _sessions.TryGetValue<UserInfo>(sessionToken, out var user);
            return user;
        }
    }
}
