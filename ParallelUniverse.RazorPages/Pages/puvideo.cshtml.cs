using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParallelUniverse.RazorPages.Data;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Pages
{
    [AllowAnonymous]
    public class puvideoModel : PageModel
    {
        private readonly ParallelUniverseContext _puctx;
        public FileResource FileResource { get; private set; }

        public puvideoModel(ParallelUniverseContext puctx)
        {
            _puctx = puctx;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            FileResource = await _puctx.FileResource.FindAsync(id);

            if (FileResource == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
