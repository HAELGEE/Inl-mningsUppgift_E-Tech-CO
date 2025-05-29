using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class MyDbContext : DbContext
{
    public DbSet<Customer> Customer { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<Shop> Shop { get; set; }
    public DbSet<CustomerSave> CustomerSave { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }
    public DbSet<OrderProduct> Product { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }
    public DbSet<ProductSubcategory> ProductSubcategory { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=E-Tech&CO;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.UserName)
            .IsUnique();
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductSubcategory>()
            .HasOne(ps => ps.ProductCategory)
            .WithMany(pc => pc.ProductSubcategories)
            .HasForeignKey(ps => ps.ProductCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Shop>()
            .HasOne(s => s.ProductCategory)
            .WithMany(pc => pc.Shop)
            .HasForeignKey(s => s.ProductCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
