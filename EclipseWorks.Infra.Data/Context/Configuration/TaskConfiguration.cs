using EclipseWorks.Domain.Tasks.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EclipseWorks.Infra.Data.Context.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.ToTable("Task");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Title)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("varchar(MAX)");

            builder.Property(x => x.DoneDate)
                .HasColumnType("datetime");

            builder.Property(x => x.CreatedAt)
               .HasColumnType("datetime")
               .IsRequired();

            builder.Property(x => x.DueDate)
               .HasColumnType("datetime")
               .IsRequired();

            builder.Property(x => x.Priority)
               .HasColumnType("int")
               .IsRequired();

            builder.Property(x => x.Progress)
               .HasColumnType("int")
               .IsRequired();

            builder.HasMany(x => x.Commentaries)
                .WithOne(x => x.Task)
                .HasForeignKey(x => x.TaskId);

            builder.HasOne(x => x.Owner)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.OwnerId);

            builder.HasOne(x => x.Project)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.ProjectId);
        }
    }
}
