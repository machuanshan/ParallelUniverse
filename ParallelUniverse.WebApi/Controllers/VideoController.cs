using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ParallelUniverse.Model;

namespace ParallelUniverse.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class VideoController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly UniverseService _universeService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<VideoController> _logger;

        public VideoController(
            UserService userService,
            UniverseService universeService,
            IConfiguration configuration,
            ILogger<VideoController> logger)
        {
            _userService = userService;
            _universeService = universeService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("videos")]
        public IEnumerable<VideoInfo> VideoList()
        {
            var dir = _configuration.GetValue<string>("VideoDir");
            var files = Directory.GetFiles(dir);
            return files.Select(f => new VideoInfo
            {
                Name = Path.GetFileName(f)
            });
        }

        [AllowAnonymous]
        [HttpGet("video/{name}")]
        public IActionResult GetVideo(string name, string token, string key)
        {
            var authorized = false;
            if(!string.IsNullOrEmpty(token))
            {
                var userInfo = _userService.GetSession(token);
                authorized = userInfo != null;
            }

            if(!string.IsNullOrEmpty(key))
            {
                var resourceName = _universeService.GetResource(key);

                if(!string.IsNullOrEmpty(resourceName))
                {
                    if(resourceName.EndsWith(name))
                    {
                        authorized = true;
                    }
                }
            }

            if(!authorized)
            {
                return Unauthorized();
            }

            var dir = _configuration.GetValue<string>("VideoDir");
            var path = Path.Combine(dir, name);
            return new PhysicalFileResult(path, "application/octet-stream")
            {
                FileDownloadName = name,
                EnableRangeProcessing = true
            };
        }
    }
}
