using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelUniverse.RazorPages.Models
{
    public class FileResource
    {
        public int Id { get; set; }

        [Display(Name = "资源名")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public long Size { get; set; }

        [StringLength(256)]
        public string Path { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public int GuestId { get; set; }

        public Guest Guest { get; set; }
    }
}
