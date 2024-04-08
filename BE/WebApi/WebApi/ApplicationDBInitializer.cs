using Microsoft.AspNetCore.Identity;
using WebApi.Models;

namespace WebApi
{
    public static class ApplicationDBInitializer
    {
        public static void SeedUsers(UserManager<UserModel> userManager)
        {
            if (userManager.FindByEmailAsync("admin@BKE.com").Result == null)
            {
                UserModel user = new UserModel
                {
                    UserName = "admin@BKE.com",
                    Email = "admin@BKE.com",
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "String!!1234").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
