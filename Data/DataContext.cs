using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;

namespace TodoListAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<ToDoItem> ToDoItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoItem>().ToTable("ToDoItems");

        }
    }
}
