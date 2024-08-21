using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Report
{
	public class CreateReportRequest
	{
        [Required]
        public int IdBooking { get; set; }
        [Required(ErrorMessage ="Nội dung đánh giá ko được bỏ trống")]
        [MaxLength(500,ErrorMessage ="Nội dung đánh giá phải dưới 500 ký tự")]
        public string Comment { get; set; }
		[Range(1, 5, ErrorMessage = "Số sao chỉ từ 1 - 5")]
		public int Rate { get; set; }
	}
}
