using KodisoftTestAssignment.Enumerators;
using KodisoftTestAssignment.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Models
{
    public class MainAppDbContext : DbContext
    {
        public MainAppDbContext(DbContextOptions<MainAppDbContext> options)
                : base(options)
        {
        }

        public virtual DbSet<RssFeed> Feeds { get; set; }
        public virtual DbSet<FeedCollection> FeedCollections { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<IdentityUser> IdentityUsers { get; set; }
    }
}
