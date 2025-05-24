using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.UserTaskDtos.Dtos
{
    public class UpdateUserTaskDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public UserTaskStatuses Status { get; set; }

        public int UserId { get; set; }

        public DateTime? DueDate { get; set; }
    }
}