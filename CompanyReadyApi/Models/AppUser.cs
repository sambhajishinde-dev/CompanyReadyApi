using System.ComponentModel.DataAnnotations;

namespace CompanyReadyApi.Models
{
    public class AppUser
    {

        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Role { get; set; } = "User";
    }
}
