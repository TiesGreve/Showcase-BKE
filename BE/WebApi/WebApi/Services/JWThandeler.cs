using System.IdentityModel.Tokens.Jwt;

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
    }
}
