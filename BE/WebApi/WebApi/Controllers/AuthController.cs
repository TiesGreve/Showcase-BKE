using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;

        public AuthController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IConfiguration configuration, DataContext dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dataContext = dataContext;
        }
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Email);
                var result = await _signInManager.PasswordSignInAsync(user.UserName, loginModel.Password!, true, false);
                var roleID = _dataContext.UserRoles.Where(r => r.UserId == user.Id).Select(r => r.RoleId);
                var role = _dataContext.Roles.Where(r => r.Id.Equals(roleID)).Select(r => r.Name);
                if (result.Succeeded)
                {
                    var authClaims = new List<Claim>
                    {
                       new Claim(ClaimTypes.Name, user.UserName),
                       new Claim(ClaimTypes.Email, user.Email),
                       new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                       new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddHours(3).ToString("ddd dd MMM yyyy HH:mm:ss")),
                       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                       new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
                       new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"])
                    };
                    if(role != null )
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    }

                    var token = GetToken(authClaims);
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
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
            return await RegisterUser(registerModel);
            
        }
        private async Task<IActionResult> RegisterUser(RegisterModel registerModel)
        {
            if(registerModel.Password != registerModel.PasswordCheck) return BadRequest(ModelState);
            if(ModelState.IsValid)
            {
                UserModel user = new()
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email
                };
                
                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            return Ok();
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
        public async Task<IActionResult> GetName()
        {
            var token = await JWThandeler.GetTokenClaims(HttpContext.Request);
            var name = token.Claims.First(claim => claim.Type == ClaimTypes.Name);
            return Ok(name);
        }
        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: authClaims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: signIn);
            
            return token;
        }
    }
}
