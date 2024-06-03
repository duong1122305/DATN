using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class ServiceManagementService : IServiceManagementService
    {
        private readonly UnitOfWork _unitOfWork;

        public ServiceManagementService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> CreateNewService(Service service)
        {
            try
            {
                await _unitOfWork.ServiceRepository.AddAsync(service);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Thêm thành công!" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Thêm ko thành công!" };
            }
        }

        public async Task<ResponseData<List<Service>>> GetAllService()
        {
            var data = await _unitOfWork.ServiceRepository.GetAllAsync();
            if (data.Count() > 0)
                return new ResponseData<List<Service>> { IsSuccess = true, Data = data.ToList() };
            else
                return new ResponseData<List<Service>> { IsSuccess = false, Error = "Chưa có dịch vụ chính nào" };
        }

        public async Task<ResponseData<Service>> GetServiceById(int id)
        {
            var query = from servicetable in await _unitOfWork.ServiceRepository.GetAllAsync()
                        where servicetable.Id == id
                        select servicetable;
            if (query.Count() > 0)
                return new ResponseData<Service> { IsSuccess = true, Data = query.FirstOrDefault() };
            else
                return new ResponseData<Service> { IsSuccess = false, Error = "Không tìm thấy" };
        }

        public async Task<ResponseData<string>> RemoveService(int idSer)
        {
            try
            {
                var service = await _unitOfWork.ServiceRepository.FindAsync(c => c.Id == idSer);
                var serviceUpdate = service.FirstOrDefault();
                serviceUpdate.IsDetele = true;
                await _unitOfWork.ServiceRepository.UpdateAsync(serviceUpdate);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Xóa thành công!" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Xóa ko thành công!" };
            }
        }

        public async Task<ResponseData<string>> UpdateService(Service service)
        {
            try
            {
                await _unitOfWork.ServiceRepository.UpdateAsync(service);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công!" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Sửa ko thành công!" };
            }
        }
    }
}
