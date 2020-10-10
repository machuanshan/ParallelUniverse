using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ParallelUniverse.RazorPages.Pages
{
    public class VideoStreamModel : PageModel
    {
        public IActionResult OnGet(string file)
        {
            return new PhysicalFileResult(file, "application/octet-stream")
            {
                FileDownloadName = Path.GetFileName(file),
                EnableRangeProcessing = true
            };
        }
    }
}
