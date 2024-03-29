﻿using Cloudy.NET.EntitySupport.Serialization;
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
        public DbSet<PageTree> PageTree { get; set; }
        public DbSet<StartPage> StartPages { get; set; }
        public DbSet<CompositeKeyTest> CompositeKeyTests { get; set; }
        public DbSet<SimpleKeyTest> SimpleKeyTests { get; set; }
        public DbSet<PropertyTestBed> PropertyTestBeds { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SimpleKeyTest>().Property(p => p.RelatedPage).SerializeIntoJson();
            modelBuilder.Entity<CompositeKeyTest>().HasKey(p => new { p.FirstPrimaryKey, p.SecondPrimaryKey });
            modelBuilder.Entity<CompositeKeyTest>().Property(p => p.RelatedObject).SerializeIntoJson();
            modelBuilder.Entity<Page>().Property(p => p.Blocks).JsonBlockConversion();
            modelBuilder.Entity<PropertyTestBed>().Property(p => p.Colors).SerializeIntoJson();
            modelBuilder.Entity<PageTree>().Property(p => p.Ancestors).SerializeIntoJson();
            modelBuilder.Entity<PageTree>().Property(p => p.Children).SerializeIntoJson();
        }
    }
}
