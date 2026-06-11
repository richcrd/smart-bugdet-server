using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Configurations;

public class UserPaymentMethodConfiguration : IEntityTypeConfiguration<UserPaymentMethod>
{
    public void Configure(EntityTypeBuilder<UserPaymentMethod> builder)
    {
        builder.ToTable("user_payment_methods");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Alias)
            .HasMaxLength(100);

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserPaymentMethods)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.PaymentMethod)
            .WithMany()
            .HasForeignKey(x => x.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.PaymentMethodId, x.Alias });
    }
}