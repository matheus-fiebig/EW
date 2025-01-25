using EclipseWorks.Domain.Commentaries.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EclipseWorks.Infra.Data.Context.Configuration
{
    public class CommentaryConfiguration : IEntityTypeConfiguration<CommentaryEntity>
    {
        public void Configure(EntityTypeBuilder<CommentaryEntity> builder)
        {
            builder.ToTable("Commentary");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Description)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnType("uniqueidentifier")
                .IsRequired();
            
            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Commentaries)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
