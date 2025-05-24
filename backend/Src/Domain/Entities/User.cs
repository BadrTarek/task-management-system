using Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<UserTask>? UserTasks { get; set; }
    }
}