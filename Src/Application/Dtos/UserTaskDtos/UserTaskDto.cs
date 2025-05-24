using Domain.Enums;

namespace Application.UserTaskDtos.Dtos
{
    public class UserTaskDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public UserTaskStatuses Status { get; set; }
        public required int UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
    }
}