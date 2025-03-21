﻿using DevTalk.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevTalk.Infrastructure.Data.Config;

public class CategoryConfigurations : IEntityTypeConfiguration<Categories>
{
    public void Configure(EntityTypeBuilder<Categories> builder)
    {
        builder.HasKey(x => x.CategoryId);
        builder.Property(x => x.CategoryId)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.CategoryName)
            .HasColumnType("varchar")
            .HasMaxLength(300);

        builder.HasMany(x => x.Posts)
            .WithMany(x => x.Categories)
            .UsingEntity<PostCategory>();

        builder.HasMany(x => x.Preferences)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
