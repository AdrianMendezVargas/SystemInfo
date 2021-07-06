using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;

namespace SystemInfo.Models.Data {
    public class ApplicationDbContext : DbContext {

        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<SystemSpecs> SystemSpecs { get; set; }

        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<SystemSpecs>()
                .HasOne(s => s.Enterprise)
                .WithMany(e => e.SystemSpecs)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SystemSpecs>()
                .HasMany(s => s.HardDisks)
                .WithOne(w => w.SystemSpecs)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

    }
}
