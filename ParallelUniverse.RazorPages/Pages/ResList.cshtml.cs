using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Pages
{
    public class ResListModel : PageModel
    {
        private IConfiguration _configuration;

        public List<FileInfo> FileInfos { get; set; }

        public ResListModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            var curDir = _configuration.GetValue<string>("ResourceRoot");
            FileInfos = Directory
                .GetFiles(curDir)
                .Select(f => new FileInfo(f))
                .ToList();
        }
    }
}
