using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.Data;
using WebApi.Interfaces.Services;
using WebApi.Models;

namespace WebApi.Services
{
    public class AdminService: IAdminService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;

        public AdminService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IConfiguration configuration, DataContext dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dataContext = dataContext;
        }

        public async Task<bool> ValidateUserIsAdmin(JwtSecurityToken token)
        {
            string TokenRole = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            var userId = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            var StoredRole = await _userManager.GetRolesAsync(user);
            bool isAdmin = TokenRole.Trim() == "Admin" && StoredRole.FirstOrDefault().Trim() == "Admin";
            if (!isAdmin)
            {
                Log.Warning($"AdminService - ValidateUserIsAdmin - User with id {userId} tried to preform admin actions");
            }
            return isAdmin;
        }
    }
}
