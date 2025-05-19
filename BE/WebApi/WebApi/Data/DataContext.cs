using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebApi.Models;

namespace WebApi.Data
{
    public class DataContext :IdentityDbContext<UserModel, IdentityRole<Guid>, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<UserModel> Users {  get; set; }
        public DbSet<Game> Games { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserModel>(entity =>
            {
                entity.HasMany(e => e.Player1Games)
                .WithOne()
                .HasForeignKey(e => e.User1)
                .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasMany(e => e.Player2Games)
                .WithOne()
                .HasForeignKey(e => e.User2)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Game>(entity =>
            {
                entity.HasOne<UserModel>()
                .WithMany(u => u.Player1Games)
                .HasForeignKey(g => g.User1)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne<UserModel>()
                .WithMany(u => u.Player2Games)
                .HasForeignKey(g => g.User2)
                .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = DateTime.UtcNow.ToString()
            });
            
            
        }
    }
}
