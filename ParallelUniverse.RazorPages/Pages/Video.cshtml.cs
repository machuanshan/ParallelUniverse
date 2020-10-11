using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ParallelUniverse.RazorPages.Data;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Pages
{
    public class VideoModel : PageModel
    {
        private readonly ParallelUniverseContext _puctx;
        private readonly ILogger<VideoModel> _logger;

        public FileResource FileResource { get; private set; }

        public VideoModel(ParallelUniverseContext puctx, ILogger<VideoModel> logger)
        {
            _puctx = puctx;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            FileResource = await _puctx.FileResource.FindAsync(id);

            if(FileResource == null)
            {
                _logger.LogWarning($"Resource not found for Id: {id}");
                return NotFound();
            }

            if(!System.IO.File.Exists(FileResource.Path))
            {
                _logger.LogError($"File not found: {FileResource.Path}");
                return NotFound();
            }

            return Page();
        }
    }
}
