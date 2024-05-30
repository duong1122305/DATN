using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Request;
using DATN.ViewModels.DTOs.Request.Service;
using DATN.ViewModels.DTOs.Response.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.IRepository
{
	public interface IServiceRepo
	{
		Task<ResponseData<PagingServiceResponse>> GetAll(PagingServiceRequest request);
		Task<ResponseData<ServiceViewModel>> Create(string serviceName);
		Task<ResponseData<bool>> Delete(DeleteRequest<int> deleteRequest);
	}
}
