using Domain.Enums;

namespace Domain.Entities
{
    public class UserTask
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public UserTaskStatuses Status { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}