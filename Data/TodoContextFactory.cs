using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoListApi.Data
{
    public class TodoContextFactory : IDesignTimeDbContextFactory<TodoContext>
    {
        public TodoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();

            try
            {
                var connectionString = GetConnectionString();
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string is not defined.");
                }

                optionsBuilder.UseSqlite(connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error configuring DbContext: {ex.Message}");
                throw;
            }

            return new TodoContext(optionsBuilder.Options);
        }

        private static string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("TODO_DB_CONNECTION") 
                ?? "Data Source=todo.db";
        }
    }
}