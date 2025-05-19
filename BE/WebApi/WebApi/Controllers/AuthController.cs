using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApi.Data;
using WebApi.Models;
using WebApi.Interfaces.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IAuthService _authService;

        public AuthController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if(ModelState.IsValid)
            {
                var result = await _authService.LoginUser(loginModel);
                if (result.GetType() == typeof(OkObjectResult)) return result;
                ModelState.AddModelError("Error", "Invalid login attempt");
                ModelState.AddModelError("Error", result.ToString());
            }
            return BadRequest(ModelState);
        }


        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if(ModelState == null) return BadRequest(ModelState);
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userManager.FindByEmailAsync(registerModel.Email);
            if (result != null) return BadRequest(registerModel.Email + " is al in gebruik");
            return await _authService.RegisterUser(registerModel);
            
        }
        
        [HttpPost("logout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Logout()
        {   
            await _signInManager.SignOutAsync();
            return Ok();
        }
        [HttpGet("Id")]
        [Authorize]
        public async Task<IActionResult> GetId()
        {
            var token = await JWThandeler.GetTokenClaims(HttpContext.Request);
            var id = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
            return Ok(id);
        }
        [HttpGet("Name")]
        [Authorize]
        public async Task<IActionResult> GetOwnName()
        {
            var token = await JWThandeler.GetTokenClaims(HttpContext.Request);
            var name = token.Claims.First(claim => claim.Type == ClaimTypes.Name);
            return Ok(name);
        }
        [HttpGet("Name/{playerID}")]
        [Authorize]
        public async Task<IActionResult> GetName(string playerID)
        {
            var user = await _userManager.FindByIdAsync(playerID);
            if (user != null) return Ok(user.UserName);
            return NotFound();
        }
        [HttpGet("Role")]
        [Authorize]
        public async Task<IActionResult> GetRole()
        {
            var token = await JWThandeler.GetTokenClaims(HttpContext.Request);
            var role = token.Claims.First(claim => claim.Type == ClaimTypes.Role);
            foreach(var claim in token.Claims) Log.Information(claim.Type + " -- "  + claim.Value);
            return Ok(role);
        }
        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            var user = await _userManager.FindByEmailAsync("admin@BKE.com");
            return Ok(user);
        }

    }
}
