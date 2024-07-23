using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN.Data.Config
{
    public class ImagePetConfig : IEntityTypeConfiguration<ImagePet>
    {
        public void Configure(EntityTypeBuilder<ImagePet> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Pet)
                .WithMany(c => c.ImagePets)
                .HasForeignKey(c => c.PetId);

            builder.Property(c => c.ImageUrl).IsRequired();

            builder.Property(c => c.IsDefault).HasDefaultValue(false);
        }
    }
}
