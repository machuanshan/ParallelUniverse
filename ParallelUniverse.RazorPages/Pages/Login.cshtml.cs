using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParallelUniverse.RazorPages.Data;

namespace ParallelUniverse.RazorPages.Pages
{
    [AllowAnonymous]
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

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            var guest = await _puc.Guest.FirstOrDefaultAsync(
                g => g.Name == UserName && g.Password == Password);

            if(guest == null)
            {
                ModelState.AddModelError("", "—È÷§ ß∞‹");
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, guest.Name),
                new Claim("FullName", guest.DisplayName),
                new Claim(ClaimTypes.Role, "Guest")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,                
                IssuedUtc = DateTimeOffset.Now,
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Redirect(returnUrl);
        }
    }
}
