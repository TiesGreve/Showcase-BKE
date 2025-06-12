using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetEnv;
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
            Env.Load();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("JWT_ISSUER"),
                audience: Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                claims: authClaims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: signIn);
            
            return token;
        }
    }
}
