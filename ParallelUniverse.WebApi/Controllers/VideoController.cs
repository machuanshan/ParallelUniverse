using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using ParallelUniverse.Model;

namespace ParallelUniverse.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet]
        public IEnumerable<VideoInfo> VideoList()
        {
            var dir = _configuration.GetValue<string>("VideoDir");
            var files = Directory.GetFiles(dir);
            return files.Select(f => new VideoInfo
            {
                Name = Path.GetFileName(f)
            });
        }

        [HttpGet("{name}")]
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
