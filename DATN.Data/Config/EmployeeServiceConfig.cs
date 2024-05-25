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
    public class EmployeeServiceConfig : IEntityTypeConfiguration<EmployeeService>
    {
        public void Configure(EntityTypeBuilder<EmployeeService> builder)
        {
            //
            builder.HasKey(c => new { c.IdCustomer, c.IdService });
            //
            builder.HasOne(c => c.Service)
                .WithMany(c => c.EmployeeServices)
                .HasForeignKey(c => c.IdService);
            //
            builder.HasOne(c => c.User)
                .WithMany(c => c.EmployeeServices)
                .HasForeignKey(c => c.IdCustomer);
        }
    }
}
