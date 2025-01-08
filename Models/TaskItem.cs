namespace TodoListApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; } // Unique identifier
        public string Name { get; set; } = string.Empty; // Task name
    }
}