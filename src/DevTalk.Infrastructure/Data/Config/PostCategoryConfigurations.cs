using DevTalk.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevTalk.Infrastructure.Data.Config;

public class PostCategoryConfigurations : IEntityTypeConfiguration<PostCategory>
{
    public void Configure(EntityTypeBuilder<PostCategory> builder)
    {
        builder.HasKey(x => new { x.PostId, x.CategoryId });
    }
}
