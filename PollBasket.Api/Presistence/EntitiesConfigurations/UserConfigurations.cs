using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PollBasket.Api.Presistence.EntitiesConfigurations;

public class UserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(p => p.FirstName).HasMaxLength(100);
        builder.Property(p=> p.LastName).HasMaxLength(100);
        builder.OwnsMany(x=> x.RefreshTokens).

            ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

    }
}
