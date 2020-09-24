using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
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
        private readonly AppData _appData;

        public UserService(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            var jsonTxt = File.ReadAllText("./appdata.json");
            _appData = JsonSerializer.Deserialize<AppData>(jsonTxt);
            _sessions = memoryCache;
        }

        public string Login(LoginInfo loginInfo)
        {
            var userInfo = _appData.Users.FirstOrDefault(
                u => u.Name == loginInfo.UserName && u.Password == loginInfo.Password);

            if(userInfo != null)
            {
                var token = Utils.GetRandomString(64);
                _sessions.Set(token, userInfo, TimeSpan.FromMinutes(10));
                return token;
            }

            return null;
        }

        public UserInfo GetSession(string sessionToken)
        {
            _sessions.TryGetValue<UserInfo>(sessionToken, out var user);
            return user;
        }
    }
}
