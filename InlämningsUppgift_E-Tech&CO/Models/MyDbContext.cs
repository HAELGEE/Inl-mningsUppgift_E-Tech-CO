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

        //modelBuilder.Entity<Shop>()
        //    .HasOne(x => x.ProductCategory)
        //    .WithMany(c => c.Shop)
        //    .HasForeignKey(x => x.ProductCategoryId)
        //    .OnDelete(DeleteBehavior.Cascade);

        // ProductCategory → ProductSubcategory (SetNull så man kan ta bort en kategori utan att ta bort subkategorier)
        //modelBuilder.Entity<ProductSubcategory>()
        //    .HasOne(psc => psc.ProductCategory)
        //    .WithMany(pc => pc.ProductSubcategories)
        //    .HasForeignKey(psc => psc.ProductCategoryId)
        //    .OnDelete(DeleteBehavior.Cascade);

        // Shop → ProductCategory (kan vara Restrict eller Cascade beroende på vad du vill)
    //    modelBuilder.Entity<Shop>()
    //        .HasOne(s => s.ProductCategory)
    //        .WithMany(pc => pc.Shop)
    //        .HasForeignKey(s => s.ProductCategoryId)
    //        .OnDelete(DeleteBehavior.Cascade);

    //    // Shop → ProductSubcategory (samma här)
    //    modelBuilder.Entity<Shop>()
    //        .HasOne(s => s.ProductSubcategory)
    //        .WithMany()
    //        .HasForeignKey(s => s.ProductSubcategoryId)
    //        .OnDelete(DeleteBehavior.Cascade);
    }
}
