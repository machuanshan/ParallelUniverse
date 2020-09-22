using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ParallelUniverse.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ParallelUniverse.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public string Login(LoginInfo loginInfo)
        {
            return _userService.Login(loginInfo);
        }

        [HttpGet("me")]
        public string GetMe()
        {
            if (Request.Headers.TryGetValue(HeaderNames.Authorization, out var authStrValue))
            {
                if (AuthenticationHeaderValue.TryParse(authStrValue, out var authHeaderValue))
                {
                    if("Bearer".Equals(authHeaderValue.Scheme, StringComparison.OrdinalIgnoreCase))
                    {
                        var userInfo = _userService.GetSession(authHeaderValue.Parameter);
                        return userInfo?.Name;
                    }
                }
            }

            return string.Empty;
        }
    }
}
