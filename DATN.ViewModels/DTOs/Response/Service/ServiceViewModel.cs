using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Response.Service
{
    public class ServiceViewModel
    {
        public int Id { get; set; } // Khóa chín
        public string Name { get; set; } // Tên dịch vụ
        public bool IsDelete { get; set; }

    }

}