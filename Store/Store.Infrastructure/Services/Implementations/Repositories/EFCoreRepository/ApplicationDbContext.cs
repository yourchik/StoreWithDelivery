using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<User, Role, Guid>(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<ProductsCategory> ProductsCategory { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureIdentityTables(modelBuilder);
            ConfigureEntityTables(modelBuilder);
        }

        private static void ConfigureEntityTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasKey(o => o.Id);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "OrderProductsManyToMany",
                    j => j.HasOne<Product>()
                        .WithMany()
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Order>()
                        .WithMany()
                        .OnDelete(DeleteBehavior.Cascade));

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Product>()
                .HasMany(o => o.Categories)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                "ProductProductsCategoryManyToMany",
                j => j.HasOne<ProductsCategory>()
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Product>()
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade));
            
            
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<ProductsCategory>().HasKey(o => o.Id);
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
        }
        
        private static void ConfigureIdentityTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}