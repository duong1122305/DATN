using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.Utilites.Check;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ServiceDetail;
using DATN.ViewModels.DTOs.ServiceDetailVM;

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
                    Price = serviceDetail.Price,
                    Duration = serviceDetail.Duration,
                    NameDetail = serviceDetail.Description,
                    CreateAt = DateTime.Now
                };

                if (CheckServiceDetail.CheckPriceIsFormat(serviceDetail.Price) == false)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Giá nhập không đúng" };
                }

                if (CheckIsNumber.Check(serviceDetail.Price.ToString()) == false)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Giá chỉ chứa ký tự là số" };
                }

                if (CheckIsNumber.Check(serviceDetail.MinWeight.ToString()) == false || CheckIsNumber.Check(serviceDetail.MaxWeight.ToString()) == false)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Cân nặng chỉ được nhập số" };
                }

                if (CheckIsNumber.Check(serviceDetail.Duration.ToString()) == false)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Thời gian chỉ chứa ký tự là số" };
                }

                await _unitOfWork.ServiceDetailRepository.AddAsync(newServiceDetail);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Thêm thành công!" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Thêm không thành công!" };
            }
        }

        public async Task<ResponseData<List<ServiceDetail>>> GetAllService()
        {
            var query = await _unitOfWork.ServiceDetailRepository.GetAllAsync();
            var result = query.OrderByDescending(c => c.Id).ToList();
            if (result.Count() > 0)
                return new ResponseData<List<ServiceDetail>> { IsSuccess = true, Data = result };
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

                return new ResponseData<ServiceDetail> { IsSuccess = false, Error = "Không có dữ liệu về dịch vụ chi tiết này" };

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
                return new ResponseData<List<ServiceDetail>> { IsSuccess = false, Error = "Không tìm thấy dịch vụ chi tiết" };
        }

        public async Task<ResponseData<string>> RemoveService(int idSer)
        {
            try
            {
                var service = await _unitOfWork.ServiceDetailRepository.FindAsync(c => c.Id == idSer);
                var serviceUpdate = service.FirstOrDefault();
                switch (serviceUpdate.IsDeleted)
                {
                    case true:
                        serviceUpdate.IsDeleted = false;
                        break;
                    case false:
                        serviceUpdate.IsDeleted = true;
                        break;
                    default:
                        break;
                }
                await _unitOfWork.ServiceDetailRepository.UpdateAsync(serviceUpdate);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Cập nhật trạng thái thành công!" };
            }
            catch (Exception)
            {

                return new ResponseData<string> { IsSuccess = false, Error = "Cập nhật trạng thái thất bại!" };

            }
        }

        public async Task<ResponseData<string>> UpdateService(int id, UpdateServiceDetailVM serviceDetail)
        {
            try
            {
                var findServiceDetailById = await _unitOfWork.ServiceDetailRepository.GetAsync(id);
                findServiceDetailById.Price = serviceDetail.Price;
                findServiceDetailById.Duration = serviceDetail.Duration;
                findServiceDetailById.NameDetail = serviceDetail.Description;
                findServiceDetailById.UpdateAt = DateTime.Now;

                if (CheckServiceDetail.CheckPriceIsFormat(serviceDetail.Price) == false)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Giá nhập không đúng" };
                }

                if (CheckIsNumber.Check(serviceDetail.Price.ToString()) == false)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Giá chỉ chứa ký tự là số" };
                }

                if (CheckIsNumber.Check(serviceDetail.MinWeight.ToString()) == false || CheckIsNumber.Check(serviceDetail.MaxWeight.ToString()) == false)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Cân nặng chỉ được nhập số" };
                }

                if (CheckIsNumber.Check(serviceDetail.Duration.ToString()) == false)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Thời gian chỉ chứa ký tự là số" };
                }

                await _unitOfWork.ServiceDetailRepository.UpdateAsync(findServiceDetailById);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công!" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Sửa không thành công!" };
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
                             ServiceDetailId = sd.Id,
                             ServiceName = sv.Name,
                             Price = sd.Price,
                             Duration = sd.Duration,
                             Description = sd.NameDetail,
                             CreatedAt = sd.CreateAt,
                             IsDeleted = sd.IsDeleted
                         }).AsQueryable();

            if (query == null) return new List<GetServiceNameVM>();

            return query.ToList();
        }

        public async Task<List<GroupByServiceName>> GroupByServiceNames()
        {
            var query = from s in await _unitOfWork.ServiceRepository.GetAllAsync()
                        join sd in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                        on s.Id equals sd.ServiceId
                        where sd.IsDeleted == false
                        group new GetServiceDetail
                        {
                            Id = sd.Id,
                            Description = sd.NameDetail,
                            Price = sd.Price,
                            Duration = sd.Duration,
                            IsActive = sd.IsDeleted // Assuming you meant !sd.IsDeleted for IsActive
                        } by s.Name into grouped
                        select new GroupByServiceName
                        {
                            ServiceName = grouped.Key,
                            ServiceDetails = grouped.ToList()
                        };
            return query.ToList();
        }
    }
}
