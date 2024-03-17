using Microsoft.AspNetCore.Identity;

namespace WebApi.Models
{
    public class UserModel:IdentityUser
    {
        public IEnumerable<Game> Games { get; set; }
    }
}
