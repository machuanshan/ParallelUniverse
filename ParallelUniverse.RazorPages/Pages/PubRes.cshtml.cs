using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParallelUniverse.RazorPages.Data;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Pages
{
    public class PubResModel : PageModel
    {
        private readonly ParallelUniverseContext _puctx;

        [BindProperty]
        public FileResource FileResource { get; set; }

        public PubResModel(ParallelUniverseContext puctx)
        {
            _puctx = puctx;
        }

        public IActionResult OnGet(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }
            
            FileResource = new FileResource
            {
                Name = Path.GetFileName(path),
                Path = path
            };


            var fileInfo = new FileInfo(path);
            FileResource.Size = fileInfo.Length;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string path)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var fileInfo = new FileInfo(path);
            FileResource.Path = path;
            FileResource.Size = fileInfo.Length;
            FileResource.CreationDate = DateTime.UtcNow;
            _puctx.FileResource.Add(FileResource);
            await _puctx.SaveChangesAsync();
            
            return RedirectToPage("./ResList");
        }
    }
}
