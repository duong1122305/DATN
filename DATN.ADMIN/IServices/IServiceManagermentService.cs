using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ServiceManager;

namespace DATN.ADMIN.IServices
{
    public interface IServiceManagermentService
    {
        public Task<ResponseData<List<Service>>> GetAll();
        public Task<ResponseData<Service>> GetById(int id);
        public Task<ResponseData<string>> Create(CreateServiceVM service);
        public Task<ResponseData<string>> Update(int id, UpdateServiceVM service);
        public Task<ResponseData<string>> Remove(int id);
    }
}
