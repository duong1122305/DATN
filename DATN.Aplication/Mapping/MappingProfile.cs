using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //CreateMap<Product, ProductViewModel>().ForMember(x => x.CategoryName, y => y.MapFrom(z => z.Category.Name));
        }
    }
}
