using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ServiceManager;

namespace DATN.Aplication.Services.IServices
{
    public interface IServiceManagementService
    {
        public Task<ResponseData<List<Service>>> GetAllService();
        public Task<ResponseData<Service>> GetServiceById(int id);
        public Task<ResponseData<string>> CreateNewService(CreateServiceVM service);
        public Task<ResponseData<string>> UpdateService(Service service);
        public Task<ResponseData<string>> RemoveService(int idSer);
    }
}