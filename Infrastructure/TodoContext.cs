using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TestProjectApi.Infrastructure.Config;
using TestProjectApi.Models;

namespace TestProjectApi.Entities;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
        
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Génération des tables
        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new TaskConfig());

        base.OnModelCreating(modelBuilder);

        /*
        modelBuilder.Entity<UserEntity>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<UserEntity>()
            .Property(u => u.Name)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<UserEntity>()
            .Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<UserEntity>()
            .Property(u => u.Email)
            .HasMaxLength(250);

        modelBuilder.Entity<UserEntity>()
          .Property(u => u.Phone)
          .HasMaxLength(25);*/


        // Table entité
        /*
        modelBuilder.Entity<TaskEntity>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<TaskEntity>()
            .Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<TaskEntity>()
            .Property(t => t.Description)
            .HasMaxLength(100);

        modelBuilder.Entity<TaskEntity>()
            .Property(t => t.IsCompleted)
            .IsRequired()
            .HasDefaultValue(false);

        modelBuilder.Entity<TaskEntity>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId);*/
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<TaskEntity> Tasks { get; set; } = null!;
}
