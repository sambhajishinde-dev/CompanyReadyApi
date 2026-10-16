using System.ComponentModel.DataAnnotations;

namespace CompanyReadyApi.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
