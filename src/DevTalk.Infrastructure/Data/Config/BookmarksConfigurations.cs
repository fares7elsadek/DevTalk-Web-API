using DevTalk.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevTalk.Infrastructure.Data.Config;

public class BookmarksConfigurations : IEntityTypeConfiguration<Bookmarks>
{
    public void Configure(EntityTypeBuilder<Bookmarks> builder)
    {
        builder.HasKey(x => x.BookmarkId);
        builder.Property(x => x.BookmarkId)
            .HasDefaultValueSql("newid()");

        builder.Property(x => x.CreatedDate)
            .HasDefaultValueSql("getdate()");
    }
}
