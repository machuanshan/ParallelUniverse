using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelUniverse.RazorPages.Models
{
    public class Guest
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Password { get; set; }
        
        [StringLength(20)]
        public string DisplayName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
