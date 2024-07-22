using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN.Data.Config
{
    public class ComboServiceConfig : IEntityTypeConfiguration<ComboService>
    {
        public void Configure(EntityTypeBuilder<ComboService> builder)
        {
            //
            builder.HasKey(x => x.Id);
            //
            builder.Property(x => x.Name)
                 .IsRequired();
            //
            builder.Property(x => x.Description)
                .IsRequired();
            //
            builder.Property(c => c.Price)
                .IsRequired();
            //
            builder.Property(c => c.CreateAt)
                .IsRequired();
            //
            builder.Property(c => c.DeleteAt)
                .IsRequired();
            //
            builder.Property(c => c.IsDeleted)
                .IsRequired();
        }
    }
}
