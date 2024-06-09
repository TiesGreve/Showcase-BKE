using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers;

public class AuthService
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly DataContext _dataContext;

    public AuthService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IConfiguration configuration, DataContext dataContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _dataContext = dataContext;
    }

    public async Task<IActionResult> LoginUser(LoginModel loginModel)
    {
        var user = await _userManager.FindByEmailAsync(loginModel.Email);
        if (user == null) return new NotFoundObjectResult("Combinatie van Wachtwoord en Email is niet correct");
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
            List<Guid> roleIDs;
            if (_dataContext.UserRoles != null)
            {
                Console.WriteLine("hij zit er in hoor");
                roleIDs = _dataContext.UserRoles.Where(r => r.UserId == user.Id).Select(r => r.RoleId).ToList();
            }
            else roleIDs = new List<Guid>();
            
            if (roleIDs.Count > 0)
            {
                var role = await _userManager.GetRolesAsync(user);
                authClaims.Add(new Claim(ClaimTypes.Role, role[0]));
            }
            else
            {
                authClaims.Add(new Claim(ClaimTypes.Role, "Gebruiker"));
            }

            var token = JWThandeler.GetToken(authClaims, _configuration);
            Console.WriteLine(token.ToString());
            return new OkObjectResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        return new NotFoundObjectResult("Combinatie van Wachtwoord en Email is niet correct");
    }
    
    public async Task<IActionResult> RegisterUser(RegisterModel registerModel)
    {
        if(registerModel.Password != registerModel.PasswordCheck) return RequestService.ReturnBadRequest(nameof(RegisterUser), "Wachtwoorden komen niet overeen!");
        
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
            return new OkResult();
        }
        return RequestService.ReturnBadRequest(nameof(RegisterUser), "Registreren niet gelukt");
    }
}