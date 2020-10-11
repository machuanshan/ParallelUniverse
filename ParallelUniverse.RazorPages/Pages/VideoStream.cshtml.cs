using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ParallelUniverse.RazorPages.Pages
{
    public class VideoStreamModel : PageModel
    {
        private readonly ILogger<VideoStreamModel> _logger;

        public VideoStreamModel(ILogger<VideoStreamModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet(string file)
        {
            _logger.LogDebug($"Send file: {file}");

            return new PhysicalFileResult(file, "application/octet-stream")
            {
                FileDownloadName = Path.GetFileName(file),
                EnableRangeProcessing = true
            };
        }
    }
}
