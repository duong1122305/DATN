using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Statistical
{
	public class Top3Statistical
	{
		/// <summary>
		/// Tên sp/ dv
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Tổng số bán ra
		/// </summary>
		public int TotalAmount { get; set; }
		/// <summary>
		/// Tổng doanh thu
		/// </summary>
		public double TotalRevenue { get; set; }
		/// <summary>
		/// Ảnh nếu có
		/// </summary>
		public string? ImgUrl { get; set; }
	}
}
