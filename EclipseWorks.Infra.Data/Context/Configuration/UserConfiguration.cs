﻿using EclipseWorks.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EclipseWorks.Infra.Data.Context.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(x => x.Role)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(x => x.CreatedAt)
               .HasColumnType("datetime")
               .IsRequired();
        }
    }
}
