using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelUniverse.RazorPages.Models
{
    public class FileResCacheEntry
    {
        public FileResource Resource { get; set; }
        public TimeSpan ExpirationTime { get; set; }
        public string ClientId { get; set; }
        public bool Used => ClientId != null;
    }
}
