using KodisoftTestAssignment.Enumerators;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace KodisoftTestAssignment.Models
{
    public class MainAppDbContext : IdentityDbContext
    {
        public MainAppDbContext(DbContextOptions<MainAppDbContext> options)
                : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<FeedCollectionFeed>()
                .HasKey(e => new { e.FeedCollectionID, e.FeedID });
            builder.Entity<Feed>()
                .Property(e => e.FeedType)
                .HasMaxLength(50)
                .HasConversion(new EnumToNumberConverter<FeedType, int>());

        }

        public virtual DbSet<Feed> Feeds { get; set; }
        public virtual DbSet<FeedCollection> FeedCollections { get; set; }
    }
}
