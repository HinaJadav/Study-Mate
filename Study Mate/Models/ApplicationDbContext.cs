using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Study_Mate.Models
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<LR> LRs { get; set; }

    }
}
