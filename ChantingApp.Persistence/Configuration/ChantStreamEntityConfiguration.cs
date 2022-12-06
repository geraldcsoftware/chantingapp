using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChantingApp.Persistence.Configuration;

public class ChantStreamEntityConfiguration : IEntityTypeConfiguration<ChantStream>
{
    public void Configure(EntityTypeBuilder<ChantStream> builder)
    {
        builder.ToTable("ChantStreams");
        builder.Property(x => x.ChantId).IsRequired();
        builder.HasKey(x => x.ChantId);
        builder.Property(x => x.Url).IsRequired().IsUnicode().HasMaxLength(4000);
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.StartTime).IsRequired();
        builder.Property(x => x.EndTime).IsRequired(false);

        builder.HasOne<Chant>()
               .WithOne(c => c.Stream)
               .HasForeignKey<ChantStream>(x => x.ChantId);
    }
}