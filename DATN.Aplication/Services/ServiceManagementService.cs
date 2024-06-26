using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ServiceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class ServiceManagementService : IServiceManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> CreateNewService(CreateServiceVM service)
        {
            try
            {
                var newService = new Service
                {
                    Name = service.Name
                };

                foreach (var i in await _unitOfWork.ServiceRepository.GetAllAsync())
                {
                    if (i.Name == service.Name)
                    {
                        return new ResponseData<string> { IsSuccess = false, Error = "Dịch vụ đã tồn tại!" };
                    }
                }

                await _unitOfWork.ServiceRepository.AddAsync(newService);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Thêm thành công!" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Thêm thất bại!" };
            }
        }

        public async Task<ResponseData<List<Service>>> GetAllService()
        {
            var data = await _unitOfWork.ServiceRepository.GetAllAsync();
            if (data.Count() > 0)
                return new ResponseData<List<Service>> { IsSuccess = true, Data = data.ToList() };
            else
                return new ResponseData<List<Service>> { IsSuccess = false, Error = "Chưa có dịch vụ chính nào", Data = new List<Service>() };
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
                switch (serviceUpdate.IsDetele)
                {
                    case false:
                        serviceUpdate.IsDetele = true;
                        break;
                    case true:
                        serviceUpdate.IsDetele = false;
                        break;
                    default:
                        break;
                }
                await _unitOfWork.ServiceRepository.UpdateAsync(serviceUpdate);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Thay đổi trạng thái thành công" };
            }
            catch (Exception)
            {

                return new ResponseData<string> { IsSuccess = false, Error = "Thay đổi trạng thái không thành công!" };

            }
        }

        public async Task<ResponseData<string>> UpdateService(int id, UpdateServiceVM service)
        {
            try
            {
                var findId = await _unitOfWork.ServiceRepository.GetAsync(id);
                findId.Name = service.Name;
                await _unitOfWork.ServiceRepository.UpdateAsync(findId);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công!" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Sửa thất bại!" };
            }
        }
    }
}
