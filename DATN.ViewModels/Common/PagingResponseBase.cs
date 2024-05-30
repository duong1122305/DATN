using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.Common
{
	public class PagingResponseBase
	{
		public int PageNumber { get; set; }// page hiện tại
		public int PageSize { get; set; }// kích thước page
		public int TotalCount { get; set; }// Tổng số bản ghi
        public int TotalPage { get; set; }// Tổng số trang
        public bool? HasNext => TotalPage > PageNumber;
		public bool? HasPrev => PageNumber > 0;
	}
}
