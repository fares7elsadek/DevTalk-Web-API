using DevTalk.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging.Abstractions;

namespace DevTalk.Infrastructure.Data.Config;

public class PostConfigurations : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.PostId);
        builder.Property(x => x.PostId)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Title)
            .HasColumnType("text")
            .HasMaxLength(300);

        builder.Property(x => x.Body)
            .HasColumnType("text");

        builder.Property(x => x.PostedAt)
            .HasDefaultValueSql("now()");

        builder.HasIndex(x => new { x.PopularityScore , x.PostedAt , x.PostId });

        builder.HasMany(x => x.Votes)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Comments)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.PostMedias)
            .WithOne(x =>x.Post)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Bookmarks)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
