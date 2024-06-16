using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.Services
{
    public class ShiftManagementService : IShiftManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShiftManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData<string>> CreateShift(ShiftView shift)
        {
            try
            {
                if (shift.From > shift.To)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Giờ bắt đầu phải nhỏ hơn giờ kết thúc!" };
                }
                else
                {
                    var shiftIden = new Shift()
                    {
                        Id = 0,
                        Name = shift.Name,
                        From = new TimeSpan(shift.From, 0, 0),
                        To = new TimeSpan(shift.To, 0, 0),
                    };
                    await _unitOfWork.ShiftRepository.AddAsync(shiftIden);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Thêm thành công" };
                }
            }
            catch (Exception e)
            {

                return new ResponseData<string> { IsSuccess = false, Error = e.Message };

            }
        }

        public async Task<ResponseData<List<Shift>>> GetListShift()
        {
            var lst = await _unitOfWork.ShiftRepository.GetAllAsync();
            if (lst.Count() > 0)
                return new ResponseData<List<Shift>> { IsSuccess = true, Data = lst.ToList() };
            else
                return new ResponseData<List<Shift>> { IsSuccess = false, Error = "Không có dữ liệu về ca làm" };
        }

        public async Task<ResponseData<string>> UpdateShift(ShiftView shift, int id)
        {
            try
            {
                if (shift.From > shift.To)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Giờ bắt đầu phải nhỏ hơn giờ kết thúc!" };
                }
                else
                {
                    var query = from shifttable in await _unitOfWork.ShiftRepository.GetAllAsync()
                                where shifttable.Id == id
                                select shifttable;
                    var shifts = query.FirstOrDefault();
                    if (query.Count() > 0)
                    {
                        shifts.From = new TimeSpan(shift.From, 0, 0);
                        shifts.To = new TimeSpan(shift.To, 0, 0);
                        shifts.Name = shift.Name;
                        await _unitOfWork.ShiftRepository.UpdateAsync(shifts);
                        await _unitOfWork.SaveChangeAsync();
                        return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công" };
                    }
                    return new ResponseData<string> { IsSuccess = false, Error = "Không có id ca làm này" };
                }
            }
            catch (Exception e)
            {

                return new ResponseData<string> { IsSuccess = false, Error = e.Message };

            }
        }
    }
}
