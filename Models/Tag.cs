namespace TodoListApi.Models
{
    public class Tag
    {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public List<TaskItem> TaskItems { get; set; } = new();
    public List<Project> Projects { get; set; } = new();
    }
}