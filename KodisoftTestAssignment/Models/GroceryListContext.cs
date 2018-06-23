using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Models
{
    public class GroceryListContext : DbContext
    {
        public GroceryListContext(DbContextOptions<GroceryListContext> options)
            : base(options)
        {
        }

        public DbSet<GroceryItem> GroceryList { get; set; }
    }

    public class GroceryItem
    {
        public long Id { get; set; }
        public string Description { get; set; }
    }
}
