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
using Microsoft.Extensions.Logging;
using ParallelUniverse.RazorPages.Data;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Pages
{
    [AllowAnonymous]
    public class puvideostreamModel : PageModel
    {
        private const string ClientId = "ClientId";
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<puvideostreamModel> _logger;

        public puvideostreamModel(IMemoryCache memoryCache, ILogger<puvideostreamModel> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public IActionResult OnGetAsync(string key)
        {
            if (!_memoryCache.TryGetValue<FileResCacheEntry>(key, out var fr))
            {
                _logger.LogWarning($"Invalid portal key: {key}");
                return NotFound();
            }

            if (Request.Cookies.TryGetValue(ClientId, out var cid) && fr.Used)
            {
                if (cid != fr.ClientId)
                {
                    _logger.LogWarning($"Client Id not found in cookie: {fr.ClientId}");
                    return NotFound();
                }
            }
            else
            {
                fr.ClientId = Guid.NewGuid().ToString("N");
                _logger.LogInformation($"Set ClientId: {fr.ClientId}");
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
