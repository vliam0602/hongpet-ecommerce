using HongPet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HongPet.Infrastructure.Database.Configuration;
public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Email).IsUnique();

        // default value
        builder.Property(x => x.Id).HasDefaultValueSql("newid()");        
        builder.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
        builder.Property(x => x.LastModificatedDate).HasDefaultValueSql("getutcdate()");               
    }
}
