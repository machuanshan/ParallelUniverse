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
        private readonly IConfiguration _configuration;
        private readonly ILogger<VideoController> _logger;

        public VideoController(
            IConfiguration configuration,
            ILogger<VideoController> logger)
        {
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

        [HttpGet("video/{name}")]
        public IActionResult GetVideo(string name)
        {
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
