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
                Log.Information("Model Valid");
                var user = await _userManager.FindByEmailAsync(loginModel.Email);
                Log.Information("User Found");
                if (user == null) return NotFound();
                var result = await _signInManager.PasswordSignInAsync(user.UserName, loginModel.Password!, true, false);
                if (result.Succeeded)
                {
                    Log.Information("Password Correct ");
                    
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
                    var roleIDs = _dataContext.UserRoles.Where(r => r.UserId == user.Id).Select(r => r.RoleId).ToList();
                    
                    if (roleIDs.Count > 0)
                    {
                        var role = await _userManager.GetRolesAsync(user);
                        authClaims.Add(new Claim(ClaimTypes.Role, role[0]));
                    }
                    else
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, "Gebruiker"));
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
            var result = await _userManager.FindByEmailAsync(registerModel.Email);
            if (result != null) return BadRequest(registerModel.Email + " is al in gebruik");
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
                    Email = registerModel.Email,
                    EmailConfirmed = true
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
            else return NotFound();
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
