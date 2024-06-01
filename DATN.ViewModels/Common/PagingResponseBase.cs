using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.Common
{
	public class PagingResponseData
	{
		public int PageIndex { get; set; }// page hiện tại
		public int PageSize { get; set; }// kích thước page
		public int TotalCount { get; set; }// Tổng số bản ghi
		public int TotalPage => (int)Math.Ceiling((decimal)TotalCount / PageSize);// Tổng số trang
        public bool? HasNext => TotalPage > PageIndex;// có next đc ko
		public bool? HasPrev => PageIndex > 0;// có lùi đc ko
        public PagingResponseData( int pageIndex, int pageSize, int tolalCount)
        {
            PageIndex = pageIndex;
			PageSize = pageSize;
			TotalCount = tolalCount;
        }
    }
}
