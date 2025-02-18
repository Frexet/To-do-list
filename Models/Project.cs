using System.Text.Json.Serialization;

namespace TodoListApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsPaused { get; set; } = false;

        [JsonIgnore]
        public List<TaskItem> TaskItems { get; set; } = new();

        public List<Tag> Tags { get; set; } = new();
    }
}