using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChantingApp.Persistence.Configuration;

public class ChantEntityConfiguration : IEntityTypeConfiguration<Chant>
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
            p.Property(x => x.Color).HasConversion<string?>(c => c == null ? null! : c.ToString(), val => Color.Parse(val, CultureInfo.InvariantCulture));
            p.Property(x => x.Colors).HasConversion<string?>(colors => colors != null ? JsonSerializer.Serialize(colors.Select(c => c.ToString()), new JsonSerializerOptions()) : null!,
                                                             val => val == null
                                                                        ? null
                                                                        : JsonSerializer.Deserialize<string[]>(val, new JsonSerializerOptions())!
                                                                                        .Select(clr => Color.Parse(clr, CultureInfo.InvariantCulture)).ToArray());
        });
    }
}