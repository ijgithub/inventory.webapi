using Inventory.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.WebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<InventoryItem> Inventory { get; set; }
        public DbSet<ItemTemplate> ItemTemplates { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
    }
}
