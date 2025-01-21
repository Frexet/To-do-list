using Microsoft.EntityFrameworkCore;
using TodoListApi.Models;

namespace TodoListApi.Data
{
    public class TodoContext : DbContext
    {
        // Constructor to inject DbContext options
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        // DbSet representing the Projects table
        public DbSet<Project> Projects { get; set; } = null!;

        // DbSet representing the Tasks table
        public DbSet<TaskItem> Tasks { get; set; } = null!;

        // DbSet representing the Tags table
        public DbSet<Tag> Tags { get; set; } = null!;

        // Fluent API configuration for advanced settings
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Project entity
            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");
                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(100); // Limit Name to 100 characters
                entity.Property(p => p.Description)
                      .HasMaxLength(500); // Limit Description to 500 characters
                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP") // Auto-set CreatedAt
                      .ValueGeneratedOnAdd();
                entity.Property(p => p.UpdatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP") // Auto-set UpdatedAt
                      .ValueGeneratedOnAddOrUpdate();
            });

            // Configure TaskItem entity
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.ToTable("Tasks");
                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(100); // Limit Name to 100 characters
                entity.Property(t => t.Description)
                      .HasMaxLength(500); // Limit Description to 500 characters
                entity.Property(t => t.DueDate)
                      .HasColumnType("datetime"); // Define column type
                entity.Property(t => t.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP") // Auto-set CreatedAt
                      .ValueGeneratedOnAdd();
                entity.Property(t => t.UpdatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP") // Auto-set UpdatedAt
                      .ValueGeneratedOnAddOrUpdate();
            });

            // Configure Tag entity
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tags");
                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(50); // Limit Name to 50 characters
                entity.Property(t => t.Description)
                      .HasMaxLength(200); // Limit Description to 200 characters
                entity.Property(t => t.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP") // Auto-set CreatedAt
                      .ValueGeneratedOnAdd();
                entity.Property(t => t.UpdatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP") // Auto-set UpdatedAt
                      .ValueGeneratedOnAddOrUpdate();
            });
        }

        // Enable logging for SQL queries (development only)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.LogTo(Console.WriteLine); // Log SQL queries to the console
            }
        }
    }
}