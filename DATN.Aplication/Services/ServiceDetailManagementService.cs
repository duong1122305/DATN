using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ServiceDetail;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class ServiceDetailManagementService : IServiceDetailManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceDetailManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> CreateNewService(CreateServiceDetailVM serviceDetail)
        {
            try
            {
                var newServiceDetail = new ServiceDetail
                {
                    ServiceId = serviceDetail.ServiceId,
                    Name = serviceDetail.Name,
                    Price = serviceDetail.Price,
                    Duration = serviceDetail.Duration,
                    Description = serviceDetail.Description,
                    CreateAt = DateTime.Now
                };
                await _unitOfWork.ServiceDetailRepository.AddAsync(newServiceDetail);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Thêm thành công!" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Thêm ko thành công!" };
            }
        }

        public async Task<ResponseData<List<ServiceDetail>>> GetAllService()
        {
            var query = await _unitOfWork.ServiceDetailRepository.GetAllAsync();
            if (query.Count() > 0)
                return new ResponseData<List<ServiceDetail>> { IsSuccess = true, Data = query.ToList() };
            else
                return new ResponseData<List<ServiceDetail>> { IsSuccess = false, Error = "Không có dịch vụ nào" };
        }

        public async Task<ResponseData<ServiceDetail>> GetServiceById(int id)
        {
            var query = from servicedetail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                        where servicedetail.Id == id
                        select servicedetail;
            if (query.Count() > 0)
                return new ResponseData<ServiceDetail> { IsSuccess = true, Data = query.FirstOrDefault() };
            else
                return new ResponseData<ServiceDetail> { IsSuccess = false, Error = "Ko có" };
        }

        public async Task<ResponseData<List<ServiceDetail>>> GetServicesByIdService(int id)
        {
            var query = from service in await _unitOfWork.ServiceRepository.GetAllAsync()
                        join serviceDetail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                        on service.Id equals serviceDetail.ServiceId
                        where serviceDetail.ServiceId == id &&
                        serviceDetail.IsDeleted == false
                        select serviceDetail;
            if (query.Count() > 0)
                return new ResponseData<List<ServiceDetail>> { IsSuccess = true, Data = query.ToList() };
            else
                return new ResponseData<List<ServiceDetail>> { IsSuccess = false, Error = "Không có" };
        }

        public async Task<ResponseData<string>> RemoveService(int idSer)
        {
            try
            {
                var service = await _unitOfWork.ServiceDetailRepository.FindAsync(c => c.Id == idSer);
                var serviceUpdate = service.FirstOrDefault();
                serviceUpdate.IsDeleted = true;
                await _unitOfWork.ServiceDetailRepository.UpdateAsync(serviceUpdate);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Xóa thành công!" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Xóa ko thành công!" };
            }
        }

        public async Task<ResponseData<string>> UpdateService(ServiceDetail serviceDetail)
        {
            try
            {
                await _unitOfWork.ServiceDetailRepository.UpdateAsync(serviceDetail);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công!" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Sửa ko thành công!" };
            }
        }

        public async Task<List<GetServiceNameVM>> GetServiceName()
        {
            var lstService = await _unitOfWork.ServiceRepository.GetAllAsync();
            var lstServiceDetail = await _unitOfWork.ServiceDetailRepository.GetAllAsync();
            var query = (from sv in lstService.ToList()
                        join sd in lstServiceDetail.ToList()
                        on sv.Id equals sd.ServiceId
                        select new GetServiceNameVM
                        {
                            ServiceDetailName = sd.Name,
                            ServiceName = sv.Name,
                            Price = sd.Price,
                            Duration = sd.Duration,
                            Description = sd.Description,
                            CreatedAt = sd.CreateAt,
                            IsDeleted = sd.IsDeleted
                        }).AsQueryable();

            if (query == null) return new List<GetServiceNameVM>();

            return query.ToList();
        }
    }
}
