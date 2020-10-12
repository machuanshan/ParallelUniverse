﻿using System;
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
            var userId = User.GetUserId();
            var query = _puctx.FileResource.Where(fr => fr.GuestId == userId);
            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(fr => fr.Name.Contains(SearchString));
            }

            FileResources = await query.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            _puctx.FileResource.Remove(new FileResource { Id = id });
            await _puctx.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
