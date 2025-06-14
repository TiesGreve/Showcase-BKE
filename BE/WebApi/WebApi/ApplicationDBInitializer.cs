using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Serilog;
using WebApi.Models;

namespace WebApi
{
    public static class ApplicationDBInitializer
    {
        public static void SeedUsers(UserManager<UserModel> userManager)
        {
            Env.Load();
            if (userManager.FindByEmailAsync(Environment.GetEnvironmentVariable("SEED_USER_EMAIL")).Result == null)
            {
                Log.Information("No seed user found, user creation started");
                UserModel user = new UserModel
                {
                    UserName = Environment.GetEnvironmentVariable("SEED_USER_EMAIL"),
                    Email = Environment.GetEnvironmentVariable("SEED_USER_EMAIL"),
                    EmailConfirmed = true
                };

                userManager.CreateAsync(user, Environment.GetEnvironmentVariable("SEED_USER_PASSWORD")).Wait();
                userManager.AddToRoleAsync(user, "Admin").Wait();
                Log.Information("Seed user Created");
                
            }
        }
    }
}
