using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Configurations;

public class PeopleConfiguration : IEntityTypeConfiguration<People>
{
    public void Configure(EntityTypeBuilder<People> builder)
    {
        builder.ToTable("people");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(x => x.LastName)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasOne(x => x.Status)
            .WithMany(x => x.Peoples)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}