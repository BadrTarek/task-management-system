
namespace Application.Dtos.UserDtos
{
    public class UserDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}