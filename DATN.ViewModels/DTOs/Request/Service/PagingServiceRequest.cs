using DATN.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Request.Service
{
	public class PagingServiceRequest:PagingRequestBase
	{
        public string? keyWord { get; set; }
    }
}
