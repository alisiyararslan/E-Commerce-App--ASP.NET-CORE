using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using EntityLayer.Concrete;

namespace DataAccessLayer.Contexts
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)//unique key usage
        {
            modelBuilder.Entity<User>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(c => c.UserName)
                .IsUnique();




            modelBuilder.Entity<FavoriteItemUser>()
                .HasOne(f => f.User)
                .WithMany(u => u.FavoriteItemUsers)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);




            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name).HasColumnName("Name");

                entity.HasData(
                    new Category { Id = 1, Name = "Electronic" },
                    new Category { Id = 2, Name = "Male" }

                );
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.ToTable("SubCategories");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.CategoryId).HasColumnName("CategoryId");
                entity.Property(e => e.Name).HasColumnName("Name");

                entity.HasData(
                    new SubCategory { Id = 1, CategoryId = 1, Name = "Phone" },
                    new SubCategory { Id = 2, CategoryId = 1, Name = "Computer & Tablet" },
                    new SubCategory { Id = 3, CategoryId = 2, Name = "Clothes" },
                    new SubCategory { Id = 4, CategoryId = 2, Name = "Shoes" }

                );
            });

            modelBuilder.Entity<CategoryDetail>(entity =>
            {
                entity.ToTable("CategoryDetails");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.CategoryId).HasColumnName("CategoryId");
                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryId");
                entity.Property(e => e.Name).HasColumnName("Name");

                entity.HasData(
                    new CategoryDetail { Id = 1, CategoryId = 1, SubCategoryId = 1, Name = "SmartPhone" },
                    new CategoryDetail { Id = 2, CategoryId = 1, SubCategoryId = 2, Name = "Laptop" },
                    new CategoryDetail { Id = 3, CategoryId = 1, SubCategoryId = 2, Name = "Monitor" }



                );
            });



            // Other entity configurations

            base.OnModelCreating(modelBuilder);


        }


        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<CategoryDetail> CategoryDetails { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Discount> Discounts { get; set; }





        public DbSet<FavoriteItemUser> FavoriteItemUsers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

    }
}
