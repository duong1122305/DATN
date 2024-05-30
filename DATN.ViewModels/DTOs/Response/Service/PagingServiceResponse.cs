using DATN.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Response.Service
{
	public class PagingServiceResponse:PagingResponseBase
	{
		public List<ServiceViewModel> Data { set; get; }= new List<ServiceViewModel>();
	}
}
