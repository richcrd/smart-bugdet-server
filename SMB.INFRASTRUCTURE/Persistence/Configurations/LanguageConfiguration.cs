using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("catalog_language");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(80)
            .IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();
    }
}