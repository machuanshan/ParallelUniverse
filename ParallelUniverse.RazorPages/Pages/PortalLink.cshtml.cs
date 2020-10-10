using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using ParallelUniverse.RazorPages.Data;

namespace ParallelUniverse.RazorPages.Pages
{
    public class PortalLinkModel : PageModel
    {
        private readonly IMemoryCache _memcache;
        private readonly ParallelUniverseContext _puctx;

        [BindProperty]
        [Required]
        public string Duration { get; set; }

        public string PortalLink { get; private set; }

        public PortalLinkModel(IMemoryCache memcache, ParallelUniverseContext puctx)
        {
            _memcache = memcache;
            _puctx = puctx;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if(!_puctx.FileResource.Any(fr => fr.Id == id))
            {
                return NotFound();
            }

            if(!TimeSpan.TryParse(Duration, out var result))
            {
                return BadRequest();
            }

            var key = await GeneratePortalKey(id, result);
            PortalLink = $"{Request.Scheme}://{Request.Host}/puvideo?key={key}";

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
                    _memcache.Set(key, fr, duration);
                    return key;
                }
            }
        }
    }
}
