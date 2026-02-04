using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TestProjectApi.Models;

namespace TestProjectApi.Entities;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<TaskEntity> Tasks { get; set; } = null!;


}
