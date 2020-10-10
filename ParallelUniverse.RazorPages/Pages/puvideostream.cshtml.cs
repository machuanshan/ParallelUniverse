using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using ParallelUniverse.RazorPages.Data;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Pages
{
    [AllowAnonymous]
    public class puvideostreamModel : PageModel
    {
        private readonly ParallelUniverseContext _puctx;
        private readonly IMemoryCache _memoryCache;

        public puvideostreamModel(ParallelUniverseContext puctx, IMemoryCache memoryCache)
        {
            _puctx = puctx;
            _memoryCache = memoryCache;
        }

        public IActionResult OnGetAsync(string key)
        {
            if (!_memoryCache.TryGetValue<FileResource>(key, out var fr))
            {
                return NotFound();
            }

            return new PhysicalFileResult(fr.Path, "application/octet-stream")
            {
                FileDownloadName = Path.GetFileName(fr.Path),
                EnableRangeProcessing = true
            };
        }
    }
}
