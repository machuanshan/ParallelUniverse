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
using Microsoft.Extensions.Logging;
using ParallelUniverse.RazorPages.Data;

namespace ParallelUniverse.RazorPages.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly ParallelUniverseContext _puc;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [BindProperty]
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Password { get; set; }

        public LoginModel(ParallelUniverseContext puc, ILogger<LoginModel> logger)
        {
            _puc = puc;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogWarning($"Bad model data, {ModelState.Values.FirstOrDefault()}");
                return Page();
            }

            var guest = await _puc.Guest.FirstOrDefaultAsync(
                g => g.Name == UserName && g.Password == Password);

            if(guest == null)
            {
                _logger.LogWarning($"Failed to login, user: {UserName}, password: {Password}");
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

            _logger.LogInformation($"Login passed for user: {UserName}");
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            
            if(string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/";
            }

            return Redirect(returnUrl);
        }
    }
}
