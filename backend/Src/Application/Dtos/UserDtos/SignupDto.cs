using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserDtos
{
    public class SignupDto
    {
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }
    }
}