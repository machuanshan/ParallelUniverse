using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ParallelUniverse.RazorPages.Pages
{
    public class PortalLinkModel : PageModel
    {
        [BindProperty]
        public string Duration { get; set; }
        
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            return null;
        }
    }
}
