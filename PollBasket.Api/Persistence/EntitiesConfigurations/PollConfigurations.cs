using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PollBasket.Api.Persistence.EntitiesConfigurations;

public class PollConfigurations : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
        builder.HasIndex(p=>p.Id).IsUnique();
        builder.Property(x=>x.Summary).HasMaxLength(100);
        builder.Property(x=>x.Title).HasMaxLength(1500);

    }
}
