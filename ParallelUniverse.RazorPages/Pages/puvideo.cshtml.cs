using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Pages
{
    [AllowAnonymous]
    public class puvideoModel : PageModel
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<puvideoModel> _logger;

        public FileResource FileResource { get; private set; }

        public puvideoModel(IMemoryCache memoryCache, ILogger<puvideoModel> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [Required]
        [BindProperty(SupportsGet = true)]
        public string Key { get; set; }

        public IActionResult OnGetAsync()
        {
            if(!_memoryCache.TryGetValue<FileResCacheEntry>(Key, out var resEntry))
            {
                _logger.LogWarning($"Invalid portal key: {Key}");
                return NotFound();
            }

            FileResource = resEntry.Resource;

            return Page();
        }
    }
}
