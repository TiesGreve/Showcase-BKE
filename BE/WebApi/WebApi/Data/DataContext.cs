using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class DataContext :IdentityDbContext<UserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Game> Games { get; set; }
    }
}
