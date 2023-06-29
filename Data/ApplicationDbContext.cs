using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasics.Models;

namespace WebApiBasics.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId);
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}   
         
  

