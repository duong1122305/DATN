﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
    public class ImageProduct
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public string UrlImage { get; set; }
        public bool IsDefault { get; set; }
        public virtual Product Product { get; set; }
    }
}
