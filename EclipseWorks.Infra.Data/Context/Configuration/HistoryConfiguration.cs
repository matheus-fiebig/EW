using EclipseWorks.Domain.Histories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EclipseWorks.Infra.Data.Context.Configuration
{
    internal class HistoryConfiguration : IEntityTypeConfiguration<HistoryEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryEntity> builder)
        {
            builder.ToTable("History");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.OriginTableName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
               .HasColumnType("datetime")
               .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(x => x.Changes)
                .HasColumnType("varchar(MAX)")
                .IsRequired();

            builder.Property(x => x.Type)
                .HasColumnType("int")
                .IsRequired();
        }
    }
}
