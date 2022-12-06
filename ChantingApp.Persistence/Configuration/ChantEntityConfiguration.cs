using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChantingApp.Persistence.Configuration;

public class ChantEntityConfiguration: IEntityTypeConfiguration<Chant>
{
    public void Configure(EntityTypeBuilder<Chant> builder)
    {
        builder.ToTable("Chants");

        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Name).IsRequired().IsUnicode().HasMaxLength(200);
        builder.Property(x => x.Description).IsRequired(false).IsUnicode().HasMaxLength(4000);
        builder.Property(x => x.CreatedByUserId)
               .HasColumnName("CreatedBy")
               .IsRequired()
               .HasMaxLength(128);
        builder.OwnsOne(x => x.VisualPreset, p =>
        {
            p.ToJson();
            p.Property(x => x.PresetType).IsRequired();
            p.Property(x => x.Color).HasConversion<string>(c => c.ToString(), val => Color.Parse(val, CultureInfo.InvariantCulture));
        });
    }

   
}