using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ParallelUniverse.RazorPages.Pages
{
    public class ResListModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<FileInfo> FileInfos { get; set; }
        public string[] Directories { get; set; }
        public string CurrentDirectory { get; set; }

        public ResListModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet(string dir)
        {
            CurrentDirectory = string.IsNullOrEmpty(dir) ? 
                _configuration.GetValue<string>("ResourceRoot") : dir;

            FileInfos = Directory
                .GetFiles(CurrentDirectory)
                .Select(f => new FileInfo(f))
                .ToList();

            Directories = Directory.GetDirectories(CurrentDirectory);
        }
    }
}
