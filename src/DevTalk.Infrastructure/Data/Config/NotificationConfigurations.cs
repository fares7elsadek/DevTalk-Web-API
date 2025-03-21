using DevTalk.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevTalk.Infrastructure.Data.Config;

public class NotificationConfigurations : IEntityTypeConfiguration<Notifications>
{
    public void Configure(EntityTypeBuilder<Notifications> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Timestamp)
            .HasDefaultValueSql("now()");
        
        builder.HasIndex(x => x.Timestamp);
    }
}
