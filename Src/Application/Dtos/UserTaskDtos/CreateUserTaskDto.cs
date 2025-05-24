using System.ComponentModel.DataAnnotations;

namespace Application.UserTaskDtos.Dtos
{
    public class CreateUserTaskDto
    {
        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public required int UserId { get; set; }

        public DateTime? DueDate { get; set; }
    }

}