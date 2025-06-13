using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Serilog;
using WebApi.Data;
using WebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Interfaces.Services;
using WebApi.Services;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IAdminService _adminService;

        public AdminController(UserManager<UserModel> userManager, IAdminService adminService)
        {
            _userManager = userManager;
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            JwtSecurityToken token = await JWThandeler.GetTokenClaims(HttpContext.Request);
            bool isAdmin = await _adminService.ValidateUserIsAdmin(token);
            if (!isAdmin)
            { 
                Log.Error("User that Requested not a Admin");
                return Forbid();
            }
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id)
        {
            JwtSecurityToken token = await JWThandeler.GetTokenClaims(HttpContext.Request);
            bool isAdmin = await _adminService.ValidateUserIsAdmin(token);
            if (isAdmin)
            {
                Log.Error("User that Requested not a Admin");
                return Forbid();
            }
            UserModel user = _userManager.Users.Where(u => u.Id == id).First();
            user.LockoutEnabled = !user.LockoutEnabled;
            user.UpdatedAt = DateTime.UtcNow;
            return Ok();
        }

    }
}
