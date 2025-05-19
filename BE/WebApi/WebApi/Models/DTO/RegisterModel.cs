using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(128)]
        public string Email { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(24)]
        public string UserName { get; set; }
        [Required]
        [MinLength(12)]

        public string Password { get; set; }
        [Required]
        [MinLength(12)]
        public string PasswordCheck { get; set; }
    }
}
