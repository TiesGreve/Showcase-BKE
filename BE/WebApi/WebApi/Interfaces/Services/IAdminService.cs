using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Interfaces.Services
{
    public interface IAdminService
    {
        public Task<bool> ValidateUserIsAdmin(JwtSecurityToken token);
    }
}
