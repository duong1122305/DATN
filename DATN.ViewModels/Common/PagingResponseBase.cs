using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.Common
{
	public class PagingResponseData
	{
		public int PageNumber { get; set; }// page hiện tại
		public int PageSize { get; set; }// kích thước page
		public int TotalCount { get; set; }// Tổng số bản ghi
        public int TotalPage { get; set; }// Tổng số trang
        public bool? HasNext => TotalPage > PageNumber;// có next đc ko
		public bool? HasPrev => PageNumber > 0;// có lùi đc ko
	}
}
