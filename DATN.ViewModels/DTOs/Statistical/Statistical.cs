using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Statistical
{
	public class Statistical
	{
		public double[] DataPiceRevenue { get; set; } = { 0, 0 };
		/// <summary>
		/// Số lượng khách hàng theo thời gian
		/// </summary>
		public CustomerStatistical CustomerStatistical { get; set; }
		/// <summary>
		/// Top 3 sản phẩm theo doanh thu
		/// </summary>
		public List<Top3Statistical> ProductRevenueStatistical { get; set;}
		/// <summary>
		/// Top 3 dịch vụ theo doanh thu
		/// </summary>
		public List<Top3Statistical> ServiceRevenueStatistical { get; set;}
		/// <summary>
		/// Top 3 sản phẩm theo số lượng bán ra
		/// </summary>
		public List<Top3Statistical> ProductQuantityStatistical { get; set;}
		/// <summary>
		/// Top 3 dịch vụ theo số lượng khách dùng
		/// </summary>
		public List<Top3Statistical> ServiceQuantityStatistical { get; set;}
    }
}
