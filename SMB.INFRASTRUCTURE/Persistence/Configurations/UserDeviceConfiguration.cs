using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Configurations;

public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
{
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.ToTable("user_devices");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ExpoPushToken)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Platform)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(x => x.ExpoPushToken).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Devices)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
