using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ParallelUniverse.RazorPages.Data;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Pages
{
    public class PortalLinkModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memcache;
        private readonly ParallelUniverseContext _puctx;
        private readonly ILogger _logger;

        [BindProperty]
        [Required]
        public string Duration { get; set; }

        public string PortalLink { get; private set; }

        public PortalLinkModel(IConfiguration configuration, IMemoryCache memcache, ParallelUniverseContext puctx, ILogger logger)
        {
            _memcache = memcache;
            _puctx = puctx;
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if(!_puctx.FileResource.Any(fr => fr.Id == id))
            {
                _logger.LogError($"No resource found for Id: {id}");
                return NotFound();
            }

            if(!TimeSpan.TryParse(Duration, out var result))
            {
                _logger.LogError($"Bad duration data: {Duration}");
                return BadRequest();
            }

            var key = await GeneratePortalKey(id, result);

            var baseUrl = _configuration.GetValue("BaseUrl", "http://localhost:8000");
            PortalLink = $"{baseUrl}/puvideo?key={key}";

            _logger.LogInformation($"Portal link generated: {PortalLink}");
            return Page();
        }

        public string GetRandomString(int dataLen)
        {
            using var rng = RandomNumberGenerator.Create();
            var randomData = new byte[dataLen];
            rng.GetNonZeroBytes(randomData);
            return new BigInteger(randomData).ToString();
        }

        public async Task<string> GeneratePortalKey(int resId, TimeSpan duration)
        {
            var fr = await _puctx.FileResource.FindAsync(resId);

            while (true)
            {
                var key = GetRandomString(8);

                if (!_memcache.TryGetValue(key, out _))
                {
                    var entry = new FileResCacheEntry
                    {
                        Resource = fr,
                        ExpirationTime = duration
                    };

                    _memcache.Set(key, entry, duration);
                    return key;
                }
            }
        }
    }
}
