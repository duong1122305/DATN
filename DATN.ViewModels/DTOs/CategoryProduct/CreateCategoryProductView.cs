﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.CategoryProduct
{
    public class CreateCategoryProductView
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int IdCategory { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
