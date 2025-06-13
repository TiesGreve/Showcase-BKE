using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class UserModel:IdentityUser<Guid>
    {
        public ICollection<Game> Player1Games {  get; set; }
        public ICollection<Game> Player2Games {  get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
