using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.ServiceManager
{
    public class UpdateServiceVM
    {
        [Required(ErrorMessage = "Tên dịch vụ không được để trống")]
        [MaxLength(50, ErrorMessage = "Tên dịch vụ không được quá 50 ký tự")]
        [MinLength(2, ErrorMessage = "Tên dịch vụ ít nhất 2 ký tự")]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}
