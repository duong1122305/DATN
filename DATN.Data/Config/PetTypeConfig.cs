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
    internal class PetTypeConfig : IEntityTypeConfiguration<PetType>
    {
        public void Configure(EntityTypeBuilder<PetType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c=>c.Name).IsRequired();
        }
    }
}
