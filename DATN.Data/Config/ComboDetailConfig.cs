using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN.Data.Config
{
    public class ComboDetailConfig : IEntityTypeConfiguration<ComboDetail>
    {
        public void Configure(EntityTypeBuilder<ComboDetail> builder)
        {
            //
            builder.HasKey(c => new { c.ServiceDetailId, c.ComboServiceId });
            //
            builder.HasOne(c => c.ServiceDetail)
                .WithMany(c => c.ComboDetails)
                .HasForeignKey(c => c.ServiceDetailId)
                .IsRequired();
            //
            builder.HasOne(c => c.ComboService)
                .WithOne(c => c.ComboDetail)
                .HasForeignKey<ComboDetail>(c => c.ComboServiceId)
                .IsRequired();

        }
    }
}
