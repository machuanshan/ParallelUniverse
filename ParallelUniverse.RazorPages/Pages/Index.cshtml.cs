using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParallelUniverse.RazorPages.Data;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ParallelUniverseContext _puctx;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ParallelUniverseContext puctx, ILogger<IndexModel> logger)
        {
            _puctx = puctx;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public List<FileResource> FileResources { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(SearchString))
            {
                FileResources = await _puctx.FileResource.ToListAsync();
            }
            else
            {
                FileResources = await _puctx.FileResource.Where(fr => fr.Name.Contains(SearchString)).ToListAsync();
            }

            return Page();
        }
    }
}
