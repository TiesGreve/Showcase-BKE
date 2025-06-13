using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(80)]
        public string Email { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MinLength(12)]
        [MaxLength(128)]
        public string Password { get; set; }
        [Required]
        [MinLength(12)]
        [MaxLength(128)]
        public string PasswordCheck { get; set; }
    }
}
