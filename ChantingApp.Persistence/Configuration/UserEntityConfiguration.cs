using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ChantingApp.Persistence.Configuration;

public class UserEntityConfiguration:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(x => x.Id).IsRequired()
               .HasValueGenerator<GuidValueGenerator>();
        builder.Property(x => x.Name).IsRequired().IsUnicode().HasMaxLength(400);
    }
}