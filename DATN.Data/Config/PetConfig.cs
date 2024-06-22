using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Config
{
    public class PetConfig : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            //
            builder.HasKey(x => x.Id);
            //
            builder.HasOne(c => c.Guest)
                .WithMany(c => c.Pets)
                .HasForeignKey(c => c.OwnerId);
            //
            builder.HasOne(c=>c.Species)
                .WithMany(c=>c.Pets)
                .HasForeignKey(c=>c.SpeciesId);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Gender)
                .IsRequired();

          
        }
    }
}
