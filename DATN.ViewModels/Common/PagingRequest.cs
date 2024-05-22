using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.Common
{
	public class PagingRequestBase
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int? TotalCount { get; set; }
		public bool? HasNext => TotalCount == null ? false : Math.Ceiling((double)TotalCount / PageSize) > PageNumber;
		public bool? HasPrev => PageNumber > 0;
	}
}
