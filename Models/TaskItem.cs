using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApi.Models
{
    // Enum to represent task priority
    public enum PriorityLevel
    {
        Low,
        Medium,
        High
    }

    // This class represents a single task in the system
    public class TaskItem
    {
        public int Id { get; set; }

        // Title is required, it's what the task is called
        [Required]
        public string Title { get; set; } = string.Empty;

        // True if the task has been completed
        public bool IsCompleted { get; set; } = false;

        // The priority of the task (Low/Medium/High)
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

        // Automatically stores when the task was created
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional deadline - user can set it
        public DateTime? Deadline { get; set; }

        // If task is completed, stores when it was done
        public DateTime? CompletedAt { get; set; }

        // The ID of the user who owns this task
        public string? UserId { get; set; }

        // Navigation property to the actual user object
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
