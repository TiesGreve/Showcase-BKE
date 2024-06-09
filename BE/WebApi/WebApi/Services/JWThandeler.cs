using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers
{
    public class JWThandeler
    {
        public async static Task<JwtSecurityToken> GetTokenClaims(HttpRequest request)
        {
            if (request.Headers.TryGetValue("Authorization", out var headerAuth))
            {
                var jwtToken = headerAuth.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var result = await Task.FromResult(
                TypedResults.Ok(new { token = jwtToken })
                );
                var token = result.Value.token;
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                return jwtSecurityToken;
            }
            return null;

        }
        public static JwtSecurityToken GetToken(IEnumerable<Claim> authClaims, IConfiguration _configuration)
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
