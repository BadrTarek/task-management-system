

namespace TaskManagementSystem.Application.Dtos
{
    public class AuthResponseDto
    {
        public required string Token { get; set; }
        public required UserDto User { get; set; }
    }
}