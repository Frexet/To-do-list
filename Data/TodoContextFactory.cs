using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoListApi.Data
{
    public class TodoContextFactory : IDesignTimeDbContextFactory<TodoContext>
    {
        public TodoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
            optionsBuilder.UseSqlite("Data Source=todo.db");

            return new TodoContext(optionsBuilder.Options);
        }
    }
}