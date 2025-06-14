using DotNetEnv;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net.Sockets;
using System.Security.Claims;
using TwoFactorAuthNet;
using WebApi.Models;
using WebApi.Models.DTO;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/2fa")]
    public class TwoFactorController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly TwoFactorAuth _tfa;

        public TwoFactorController(UserManager<UserModel> userManager)
        {
            Env.Load();
            _userManager = userManager;
            _tfa = new TwoFactorAuth(Environment.GetEnvironmentVariable("JWT_ISSUER"));
        }

        [HttpGet("setup/{email}")]
        public async Task<IActionResult> Setup(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized();

            if (user.TwoFactorSecret == null)
            {
                var secret = _tfa.CreateSecret();
                user.TwoFactorSecret = secret;
                user.TwoFactorEnabled = true;
                await _userManager.UpdateAsync(user);
            }

            var qrCode = _tfa.GetQrCodeImageAsDataUri(user.Email, user.TwoFactorSecret);
            return Ok(qrCode);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] TwoFactorModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user.TwoFactorSecret == null) return Unauthorized();

            var isValid = _tfa.VerifyCode(user.TwoFactorSecret, request.Code);
            if (!isValid) return Unauthorized();

            // Optional: set a flag in DB that 2FA is confirmed

            return Ok();
        }
    }
}
