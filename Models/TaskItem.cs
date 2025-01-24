using System.Text.Json.Serialization;

namespace TodoListApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsCompleted { get; set; }
        public int ProjectId { get; set; }

        [JsonIgnore]
        public Project Project { get; set; } = new Project();

        public List<Tag> Tags { get; set; } = new();
    }
}