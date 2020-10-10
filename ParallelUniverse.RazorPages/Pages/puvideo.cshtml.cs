using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class puvideoModel : PageModel
    {
        private readonly ParallelUniverseContext _puctx;
        private readonly IMemoryCache _memoryCache;

        public FileResource FileResource { get; private set; }

        public puvideoModel(ParallelUniverseContext puctx, IMemoryCache memoryCache)
        {
            _puctx = puctx;
            _memoryCache = memoryCache;
        }

        [Required]
        [BindProperty(SupportsGet = true)]
        public string Key { get; set; }

        public IActionResult OnGetAsync()
        {
            if(!_memoryCache.TryGetValue<FileResource>(Key, out var res))
            {
                return NotFound();
            }

            FileResource = res;

            if (FileResource == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
