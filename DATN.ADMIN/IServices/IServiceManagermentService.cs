using DATN.Data.Entities;
using DATN.ViewModels.Common;

namespace DATN.ADMIN.IServices
{
    public interface IServiceManagermentService
    {
        public Task<ResponseData<List<Service>>> GetAll();
        public Task<ResponseData<Service>> GetById(int id);
        public Task<ResponseData<string>> Create(Service service);
        public Task<ResponseData<string>> Update(Service service);
        public Task<ResponseData<string>> Remove(int id);
    }
}
