using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN.Data.Config
{
    public class PetSeciesConfig : IEntityTypeConfiguration<PetSpecies>
    {
        public void Configure(EntityTypeBuilder<PetSpecies> builder)
        {
            //
            builder.HasKey(x => x.Id);
            //
            builder.HasOne(c => c.PetType)
                .WithMany(c => c.Species)
                .HasForeignKey(c => c.PetTypeId);

            builder.Property(x => x.Name)
                .IsRequired();

        }
    }
}
