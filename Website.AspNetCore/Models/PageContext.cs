using Cloudy.CMS.ContentSupport.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.AspNetCore.Models.Blocks;

namespace Website.AspNetCore.Models
{
    public class PageContext : DbContext
    {
        public PageContext(DbContextOptions<PageContext> options) : base(options) { }

        public DbSet<Page> Pages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Page>()
                .Property(e => e.SidebarBlocks)
                .HasConversion(new PolymorphicValueConverter<IEnumerable<ISidebarBlock>>());
            base.OnModelCreating(modelBuilder);
        }
    }
}
