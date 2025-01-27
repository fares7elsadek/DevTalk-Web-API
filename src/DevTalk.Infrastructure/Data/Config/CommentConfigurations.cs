using DevTalk.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevTalk.Infrastructure.Data.Config;

public class CommentConfigurations : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(x => x.CommentId);
        builder.Property(x => x.CommentId)
            .HasDefaultValueSql("newid()");

        builder.Property(x => x.CommentText)
            .HasColumnType("text");

        builder.Property(x => x.CommentedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasMany(x => x.Votes)
            .WithOne(x => x.Comment)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
