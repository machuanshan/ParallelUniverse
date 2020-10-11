using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private const string ClientId = "ClientId";
        private readonly IMemoryCache _memoryCache;

        public puvideostreamModel(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult OnGetAsync(string key)
        {
            if (!_memoryCache.TryGetValue<FileResCacheEntry>(key, out var fr))
            {
                return NotFound();
            }

            if (Request.Cookies.TryGetValue(ClientId, out var cid))
            {
                if (cid != fr.ClientId)
                {
                    return NotFound();
                }
            }
            else
            {
                fr.ClientId = Guid.NewGuid().ToString("N");
                Response.Cookies.Append(ClientId, fr.ClientId, new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,                    
                });
            }

            return new PhysicalFileResult(fr.Resource.Path, "application/octet-stream")
            {
                FileDownloadName = Path.GetFileName(fr.Resource.Path),
                EnableRangeProcessing = true
            };
        }
    }
}
