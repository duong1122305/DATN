using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.Enum;
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
                            VoucherName = voucherView.VoucherName,
                            VoucherCode = voucherView.VoucherCode.ToUpper(),
                            Created = DateTime.Now,
                            StartDate = voucherView.StartDate,
                            EndDate = voucherView.EndDate,
                            DiscountPercent = voucherView.DiscountPercent,
                            MaxMoneyDiscount = voucherView.MaxMoneyDiscount,
                            MinMoneyApplicable = voucherView.MinMoneyApplicable,
                            Description = voucherView.Description,
                            Quantity = voucherView.Quantity,
                        };
                        var dateNow = DateTime.Now;
                        if (voucher.StartDate.CompareTo(dateNow.Date) < 0)
                        {
                            return new ResponseData<string> { IsSuccess = false, Error = "Ngày bắt đầu phải lớn hơn ngày hiện tại" };
                        }
                        else
                        {
                            if (voucher.StartDate.CompareTo(voucher.EndDate) > 0)
                            {
                                return new ResponseData<string> { IsSuccess = false, Error = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc" };
                            }
                            else
                            {
                                if (voucher.StartDate.CompareTo(dateNow.Date) > 0)
                                {
                                    voucher.Status = VoucherStatus.NotOccur;
                                }
                                else
                                {
                                    if (voucher.StartDate.Hour > dateNow.Hour)
                                    {
                                        voucher.Status = VoucherStatus.GoingOn;
                                    }
                                    else
                                    {
                                        voucher.Status = VoucherStatus.NotOccur;
                                    }
                                }
                                await _unitOfWork.DiscountRepository.AddAsync(voucher);
                                await _unitOfWork.SaveChangeAsync();
                                return new ResponseData<string> { IsSuccess = true, Data = "Đã thêm voucher thành công!!" };
                            }
                        }
                    }
                    return new ResponseData<string> { IsSuccess = false, Error = "Voucher nhập trùng voucher code đã có!!!" };
                }
                catch (Exception)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Quá trình thêm voucher sảy ra lỗi!!" };
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
                        voucher.VoucherName = voucherView.VoucherName;
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
                    return new ResponseData<string> { IsSuccess = false, Error = "Voucher nhập trùng voucher code đã có!!!" };
                }
                catch (Exception)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Quá trình thêm voucher sảy ra lỗi!!" };
                }
            }
            return new ResponseData<string> { IsSuccess = false, Error = "Không có id này" };
        }
        public async Task<ResponseData<List<VoucherView>>> GetAllVoucher()
        {
            var dateNow = DateTime.Now;
            var query = from discount in await _unitOfWork.DiscountRepository.GetAllAsync()
                        select discount;
            List<Discount> listcheck = new List<Discount>();
            foreach (var item in query.ToList())
            {
                if (item.EndDate < dateNow)
                {
                    if (item.Status != VoucherStatus.Expired)
                    {
                        item.Status = VoucherStatus.Expired;
                        listcheck.Add(item);
                    }
                }
                else if (item.StartDate.CompareTo(dateNow) <= 0 && item.EndDate.CompareTo(dateNow) > 0)
                {
                    if (item.DeleteAt == null)
                    {
                        item.Status = VoucherStatus.GoingOn;
                        listcheck.Add(item);
                    }
                }
                else
                {
                    if (item.DeleteAt == null)
                    {
                        item.Status = VoucherStatus.NotOccur;
                        listcheck.Add(item);
                    }
                }
            }
            await _unitOfWork.DiscountRepository.UpdateRangeAsync(listcheck);

            if (query.Count() > 0)
            {
                var list = from discount in query
                           select new VoucherView
                           {
                               Id = discount.Id,
                               VoucherName = discount.VoucherName,
                               VoucherCode = discount.VoucherCode,
                               StartDate = discount.StartDate,
                               EndDate = discount.EndDate,
                               DiscountPercent = discount.DiscountPercent,
                               MaxMoneyDiscount = discount.MaxMoneyDiscount,
                               MinMoneyApplicable = discount.MinMoneyApplicable,
                               Description = discount.Description,
                               Quantity = discount.Quantity,
                               Status = discount.Status
                           };
                return new ResponseData<List<VoucherView>>
                {
                    IsSuccess = true,
                    Data = list.ToList()
                };
            }
            else
                return new ResponseData<List<VoucherView>> { IsSuccess = false, Data = new List<VoucherView>(), Error = "Chưa có voucher nào" };
        }

        public async Task<ResponseData<string>> ExpiresVoucher(int id)
        {
            var query = (from voucher in await _unitOfWork.DiscountRepository.GetAllAsync()
                         where voucher.Id == id
                         select voucher).FirstOrDefault();
            if (query == null) return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy voucher có id này!!" };
            else
            {
                try
                {
                    query.DeleteAt = DateTime.Now;
                    query.Status = VoucherStatus.Expired;
                    await _unitOfWork.DiscountRepository.UpdateAsync(query);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Xóa vé trước hạn thành công" };
                }
                catch (Exception e)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = e.Message };
                }
            }
        }
    }
}