using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebApi.Models;

namespace WebApi.Data
{
    public class DataContext :IdentityDbContext<UserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<UserModel> Users {  get; set; }
        public DbSet<Game> Games { get; set; }

    }
}
