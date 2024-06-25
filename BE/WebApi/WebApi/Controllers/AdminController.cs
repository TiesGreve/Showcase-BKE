using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Serilog;
using WebApi.Data;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;

        public AdminController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IConfiguration configuration, DataContext dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dataContext = dataContext;
        }
        // GET: api/<AdminController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var token = await JWThandeler.GetTokenClaims(HttpContext.Request);
            var role = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            if(role != "Admin")
            {
                Log.Error("User that Requested not a Admin");
                return Forbid();
            }
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        // PUT api/<AdminController>/5
        [HttpPut("{id}")]
        public void Put(Guid id)
        {
            var user = _userManager.Users.Where(u => u.Id == id).First();
            user.LockoutEnabled = !user.LockoutEnabled;
        }

    }
}
