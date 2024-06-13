using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class VoucherManagementService : IVoucherManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VoucherManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> CreateVoucher(VoucherView voucherView)
        {
            if (voucherView != null)
            {
                try
                {
                    var query = from discount in await _unitOfWork.DiscountRepository.GetAllAsync()
                                where voucherView.VoucherCode == discount.VoucherCode
                                select discount;
                    if (query.Count() == 0)
                    {
                        var voucher = new Discount()
                        {
                            VoucherCode = voucherView.VoucherCode,
                            Created = DateTime.Now,
                            StartDate = voucherView.StartDate,
                            EndDate = voucherView.EndDate,
                            DiscountPercent = voucherView.DiscountPercent,
                            MaxMoneyDiscount = voucherView.MaxMoneyDiscount,
                            MinMoneyApplicable = voucherView.MinMoneyApplicable,
                            Description = voucherView.Description,
                            Quantity = voucherView.Quantity,
                        };
                        await _unitOfWork.DiscountRepository.AddAsync(voucher);
                        await _unitOfWork.SaveChangeAsync();
                        return new ResponseData<string> { IsSuccess = true, Data = "Đã thêm voucher thành công!!" };
                    }
                    return new ResponseData<string> { IsSuccess = false, Data = "Voucher nhập trùng voucher code đã có!!!   " };
                }
                catch (Exception)
                {
                    return new ResponseData<string> { IsSuccess = true, Data = "Quá trình thêm voucher sảy ra lỗi!!" };
                }
            }
            return new ResponseData<string> { IsSuccess = false, Error = "Chưa có data" };
        }
        public async Task<ResponseData<string>> UpdateVoucher(VoucherView voucherView)
        {
            if (voucherView != null)
            {
                try
                {
                    var query = from discount in await _unitOfWork.DiscountRepository.GetAllAsync()
                                where voucherView.Id == discount.Id
                                select discount;
                    if (query.Count() == 1)
                    {
                        var voucher = query.FirstOrDefault();
                        voucher.VoucherCode = voucherView.VoucherCode;
                        voucher.StartDate = voucherView.StartDate;
                        voucher.EndDate = voucherView.EndDate;
                        voucher.DiscountPercent = voucherView.DiscountPercent;
                        voucher.MaxMoneyDiscount = voucherView.MaxMoneyDiscount;
                        voucher.MinMoneyApplicable = voucherView.MinMoneyApplicable;
                        voucher.Description = voucherView.Description;
                        voucher.Quantity = voucherView.Quantity;
                        await _unitOfWork.DiscountRepository.UpdateAsync(voucher);
                        await _unitOfWork.SaveChangeAsync();
                        return new ResponseData<string> { IsSuccess = true, Data = "Sửa voucher thành công!!" };
                    }
                    return new ResponseData<string> { IsSuccess = false, Data = "Voucher nhập trùng voucher code đã có!!!   " };
                }
                catch (Exception)
                {
                    return new ResponseData<string> { IsSuccess = true, Data = "Quá trình thêm voucher sảy ra lỗi!!" };
                }
            }
            return new ResponseData<string> { IsSuccess = false, Error = "Không có id này" };
        }
        public async Task<ResponseData<List<VoucherView>>> GetAllVoucher()
        {
            var query = from discount in await _unitOfWork.DiscountRepository.GetAllAsync()
                        select new VoucherView
                        {
                            Id=discount.Id,
                            VoucherCode = discount.VoucherCode,
                            StartDate = discount.StartDate,
                            EndDate = discount.EndDate,
                            DiscountPercent = discount.DiscountPercent,
                            MaxMoneyDiscount = discount.MaxMoneyDiscount,
                            MinMoneyApplicable = discount.MinMoneyApplicable,
                            Description = discount.Description,
                            Quantity = discount.Quantity
                        };
            if (query.Count() > 0)
                return new ResponseData<List<VoucherView>>
                {
                    IsSuccess = true,
                    Data = query.ToList()
                };
            else
                return new ResponseData<List<VoucherView>> { IsSuccess = false, Error = "Chưa có voucher nào" };
        }
    }
}