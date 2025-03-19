using DevTalk.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevTalk.Infrastructure.Data.Config;

public class PostVotesConfigurations : IEntityTypeConfiguration<PostVotes>
{
    public void Configure(EntityTypeBuilder<PostVotes> builder)
    {
        builder.HasKey(x => x.VoteId);
        builder.Property(x => x.VoteId)
            .HasDefaultValueSql("gen_random_uuid()");
    }
}
