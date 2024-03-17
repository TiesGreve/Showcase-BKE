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

        public AuthController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
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

                if (result.Succeeded)
                {
                    var authClaims = new List<Claim>
                    {
                       new Claim(ClaimTypes.Name, user.UserName),
                       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                    var token = GetToken(authClaims);
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                ModelState.AddModelError("", "Invalid login attempt");
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

        private string HashPassword(string password)
        {
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            var salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }
        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                authClaims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: signIn);

            return token;
        }
    }
}
