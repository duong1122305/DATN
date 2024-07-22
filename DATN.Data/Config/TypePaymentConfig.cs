using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN.Data.Config
{
    public class TypePaymentConfig : IEntityTypeConfiguration<TypePayment>
    {
        public void Configure(EntityTypeBuilder<TypePayment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name)
                .IsRequired();
        }
    }
}
