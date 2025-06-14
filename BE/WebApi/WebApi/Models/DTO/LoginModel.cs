using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [MinLength(1)]
        [MaxLength(80)]
        [RegularExpression(@"^(?=.{6,128}$)[\w.-]+@([\w-]+\.)+[\w-]{2,6}$")]
        public string Email { get; set; }
        [Required]
        [MinLength(12)]
        [MaxLength(128)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$")]
        public string Password { get; set; }
    }
}
