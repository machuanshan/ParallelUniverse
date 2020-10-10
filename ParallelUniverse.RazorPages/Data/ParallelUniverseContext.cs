using Microsoft.EntityFrameworkCore;
using ParallelUniverse.RazorPages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelUniverse.RazorPages.Data
{
    public class ParallelUniverseContext : DbContext
    {
        public ParallelUniverseContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<FileResource> FileResource { get; set; }
        public DbSet<Guest> Guest { get; set; }
    }
}
