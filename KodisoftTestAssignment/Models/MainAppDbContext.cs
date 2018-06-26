using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        }

        public virtual DbSet<Feed> Feeds { get; set; }
        public virtual DbSet<FeedCollection> FeedCollections { get; set; }
    }
}
