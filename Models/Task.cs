namespace TodoListApi.Models
{
    public class Task
    {
        public int Id { get; set; } // Unique identifier
        public string Name { get; set; } = string.Empty; // Task name
    }
}