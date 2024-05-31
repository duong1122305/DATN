using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.Common
{
    public class DeleteRequest<T>
    {
        [Required(ErrorMessage = "Hãy nhập ID cần xoá")]
        public T ID { get; set; }
        [Required(ErrorMessage = "Chưa có giá trị xoá hay ko")]
        public bool IsDelete { get; set; }
    }
}
