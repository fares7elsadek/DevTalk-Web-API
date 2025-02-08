using DevTalk.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevTalk.Infrastructure.Data.Config;

public class CategoryConfigurations : IEntityTypeConfiguration<Categories>
{
    public void Configure(EntityTypeBuilder<Categories> builder)
    {
        builder.HasKey(x => x.CategoryId);
        builder.Property(x => x.CategoryId)
            .HasDefaultValueSql("newid()");

        builder.Property(x => x.CategoryName)
            .HasColumnType("nvarchar")
            .HasMaxLength(300);

        builder.HasMany(x => x.Posts)
            .WithMany(x => x.Categories)
            .UsingEntity<PostCategory>();
    }
}
