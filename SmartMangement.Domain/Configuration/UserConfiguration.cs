using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartMangement.Domain.Models;

namespace SmartMangement.Domain.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEnity>
    {
        public void Configure(EntityTypeBuilder<UserEnity> builder)
        {
            builder.ToTable("CFG_USER", "dbo");
            builder.HasKey(e=> e.ID);
            builder.Property(e=>e.FirstName).HasColumnName("").IsRequired();
            builder.Property(e => e.LastName).HasColumnName("").IsRequired();
            builder.Property(e => e.UserName).HasColumnName("").IsRequired();
            builder.Property(e => e.Password).HasColumnName("").IsRequired();
            builder.Property(e => e.ExpireDate).HasColumnName("").IsRequired();
            builder.Property(e => e.InsertedDate).HasColumnName("").IsRequired();
            builder.Property(e => e.InsertedBy).HasColumnName("").IsRequired();
            builder.Property(e => e.LastUpdatedBy).HasColumnName("").IsRequired();
            builder.Property(e => e.LastUpdatedDate).HasColumnName("").IsRequired();
            builder.Property(e => e.Active).HasColumnName("").IsRequired();
            builder.Property(e => e.ClientId).HasColumnName("").IsRequired();

            //builder.HashOne(x=>x.a)


        }
    }
}
