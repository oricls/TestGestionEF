using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestProjectApi.Entities;

namespace TestProjectApi.Infrastructure.Config
{
    public class TaskConfig : IEntityTypeConfiguration<TaskEntity>
    {

        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId);
        }
    }
}
