using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Pages
{
    [AllowAnonymous]
    public class puvideoModel : PageModel
    {
        private readonly IMemoryCache _memoryCache;

        public FileResource FileResource { get; private set; }

        public puvideoModel(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [Required]
        [BindProperty(SupportsGet = true)]
        public string Key { get; set; }

        public IActionResult OnGetAsync()
        {
            if(!_memoryCache.TryGetValue<FileResCacheEntry>(Key, out var resEntry))
            {
                return NotFound();
            }

            FileResource = resEntry.Resource;

            return Page();
        }
    }
}
