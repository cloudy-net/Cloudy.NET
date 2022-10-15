using Cloudy.CMS.ContentSupport.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestWebsite.Models
{
    public class PageContext : DbContext
    {
        public PageContext(DbContextOptions<PageContext> options) : base(options) { }

        public DbSet<Page> Pages { get; set; }
        public DbSet<CompositeKeyTest> CompositeKeyTests { get; set; }
        public DbSet<SimpleKeyTest> SimpleKeyTests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SimpleKeyTest>().Property(p => p.RelatedPage).JsonConversion();
            modelBuilder.Entity<CompositeKeyTest>().HasKey(p => new { p.FirstPrimaryKey, p.SecondPrimaryKey });
            modelBuilder.Entity<CompositeKeyTest>().Property(p => p.RelatedObject).JsonConversion();
        }
    }
}
