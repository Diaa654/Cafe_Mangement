using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class CafeDbContext: IdentityDbContext<User, IdentityRole<int>, int>
    {
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatusLog> OrderStatusLogs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Table> Tables { get; set; }
        public CafeDbContext(DbContextOptions<CafeDbContext> options) : base(options)
        {
        }
        override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole<int>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
            builder.Ignore<IdentityUserClaim<int>>();
            builder.Ignore<IdentityRoleClaim<int>>();
            // Configure relationships and constraints

            #region User
            builder.Entity<User>()
                    .HasMany(u => u.Invoices)
                    .WithOne(i => i.User)
                    .HasForeignKey(uc => uc.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<User>()
                .HasMany(u => u.OrderStatusLog)
                .WithOne()
                .HasForeignKey(osl => osl.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<User>()
                .HasMany(u => u.OrderStatusLog)
                .WithOne(osl => osl.User)
                .HasForeignKey(osl => osl.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Invoice
            builder.Entity<Invoice>()
                    .HasOne(i => i.Table)
                    .WithMany()
                    .HasForeignKey(i => i.TableId);
            builder.Entity<Invoice>()
                .HasMany(i => i.Orders)
                .WithOne(o => o.Invoice)
                .HasForeignKey(o => o.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Product
            builder.Entity<Product>()
                   .HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId);
            builder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId);
            #endregion

            #region Order
            builder.Entity<Order>()
                    .HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Order>()
                .HasMany(o => o.StatusLogs)
                .WithOne()
                .HasForeignKey(osl => osl.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
            #region Table
            builder.Entity<Table>()
                    .HasMany(t => t.Invoices)
                    .WithOne(i => i.Table)
                    .HasForeignKey(i => i.TableId)
                    .OnDelete(DeleteBehavior.NoAction);
            #endregion


        }
            

    }
}
