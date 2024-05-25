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
