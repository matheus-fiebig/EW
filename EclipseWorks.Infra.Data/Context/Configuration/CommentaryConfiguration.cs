using EclipseWorks.Domain.Commentaries.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .HasMaxLength(500);

            builder.Property(x => x.UserId)
                .HasColumnType("uniqueidentifier");
            
            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetime");
        }
    }
}
