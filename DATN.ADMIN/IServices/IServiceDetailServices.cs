using DATN.Data.Entities;
using DATN.ViewModels.Common;

namespace DATN.ADMIN.IServices
{
    public interface IServiceDetailServices
    {
        public Task<ResponseData<List<ServiceDetail>>> GetAll();
        public Task<ResponseData<ServiceDetail>> GetById(int id);
        public Task<ResponseData<string>> Create(ServiceDetail serviceDetail);
        public Task<ResponseData<string>> Update(ServiceDetail serviceDetail);
        public Task<ResponseData<string>> Remove(int id);
        public Task<ResponseData<List<ServiceDetail>>> GetServiceDetailByServiceId(int id);
    }
}
