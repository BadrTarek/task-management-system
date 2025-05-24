using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Application.UserDtos.Dtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}