using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ServiceDetail;

namespace DATN.ADMIN.IServices
{
    public interface IServiceDetailServices
    {
        public Task<ResponseData<List<ServiceDetail>>> GetAll();
        public Task<ResponseData<ServiceDetail>> GetById(int id);
        public Task<ResponseData<string>> Create(CreateServiceDetailVM serviceDetail);
        public Task<ResponseData<string>> Update(ServiceDetail serviceDetail);
        public Task<ResponseData<string>> Remove(int id);
        public Task<ResponseData<List<ServiceDetail>>> GetServiceDetailByServiceId(int id);
        public Task<string> GetSerivceNameByServiceId(int id);
    }
}
