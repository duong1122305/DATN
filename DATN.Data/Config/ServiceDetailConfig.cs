using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN.Data.Config
{
    public class ServiceDetailConfig : IEntityTypeConfiguration<ServiceDetail>
    {
        public void Configure(EntityTypeBuilder<ServiceDetail> builder)
        {
            //
            builder.HasKey(x => x.Id);
            //
            builder.HasOne(c => c.Service)
                .WithMany(c => c.ServiceDetails)
                .HasForeignKey(c => c.ServiceId);

            builder.Property(x => x.Price)
                .IsRequired();

            builder.Property(x => x.Duration)
                .IsRequired();

            builder.Property(x => x.CreateAt)
                .IsRequired();
        }
    }
}
