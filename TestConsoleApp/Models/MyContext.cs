using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestConsoleApp.Models
{
    public class PageContext : DbContext
    {
        public PageContext(DbContextOptions<PageContext> options) : base(options) { }

        public DbSet<Page> Pages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Page>().Property(p => p.Blocks).JsonBlockConversion();
        }
    }
}
