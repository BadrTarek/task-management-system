using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;



namespace Data.Database.Models
{
    [Table("user_tasks")]
    [Index(nameof(UserId))]
    [Index(nameof(Status))]
    [Index(nameof(UserId), nameof(Status))]
    public class UserTask
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("title")]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [Column("due_date")]
        public DateTime DueDate { get; set; }

        [Required]
        [Column("status")]
        public UserTaskStatuses Status { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }

        public virtual User? User { get; set; }
    }
}