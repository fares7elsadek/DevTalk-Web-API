using DevTalk.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevTalk.Infrastructure.Data.Config;

public class CommentVotesConfiguration : IEntityTypeConfiguration<CommentVotes>
{
    public void Configure(EntityTypeBuilder<CommentVotes> builder)
    {
        builder.HasKey(x => x.CommentId);
        builder.Property(x => x.CommentId)
            .HasDefaultValueSql("newid()");
    }
}
