using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParallelUniverse.RazorPages.Data;

namespace ParallelUniverse.RazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ParallelUniverseContext _puc;

        [BindProperty]
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [BindProperty]
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Password { get; set; }

        public LoginModel(ParallelUniverseContext puc)
        {
            _puc = puc;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            await Task.CompletedTask;
            return RedirectToPage("./Index");
        }
    }
}
