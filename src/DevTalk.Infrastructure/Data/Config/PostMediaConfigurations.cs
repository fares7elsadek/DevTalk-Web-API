using DevTalk.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevTalk.Infrastructure.Data.Config;

public class PostMediaConfigurations : IEntityTypeConfiguration<PostMedia>
{
    public void Configure(EntityTypeBuilder<PostMedia> builder)
    {
        builder.HasKey(x => x.PostMediaId);
        builder.Property(x => x.PostMediaId)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.MediaUrl)
            .HasColumnType("text");

    }
}
