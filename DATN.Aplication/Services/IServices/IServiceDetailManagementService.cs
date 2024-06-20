using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ServiceDetail;

namespace DATN.Aplication.Services.IServices
{
    public interface IServiceDetailManagementService
    {
        public Task<ResponseData<List<ServiceDetail>>> GetAllService();
        public Task<ResponseData<List<ServiceDetail>>> GetServicesByIdService(int id);
        public Task<ResponseData<ServiceDetail>> GetServiceById(int id);
        public Task<ResponseData<string>> CreateNewService(CreateServiceDetailVM service);
        public Task<ResponseData<string>> UpdateService(ServiceDetail service);
        public Task<ResponseData<string>> RemoveService(int idSer);
    }
}