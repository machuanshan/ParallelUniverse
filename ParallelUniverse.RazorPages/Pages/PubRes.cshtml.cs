using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Pages
{
    public class PubResModel : PageModel
    {
        [BindProperty]
        public FileResource FileResource { get; set; }

        public void OnGet(string path)
        {
            FileResource = new FileResource
            {
                Name = Path.GetFileName(path),
                Path = path
            };
        }

        public async Task<IActionResult> OnPostAsync(string path)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            FileResource.Path = path;
            //save file
            await Task.CompletedTask;
            return RedirectToPage("./ResList");
        }
    }
}
