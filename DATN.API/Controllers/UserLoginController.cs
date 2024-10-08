﻿using DATN.Aplication.Services.IServices;
using DATN.Aplication.System;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IAuthenticate _userrepo;
        private readonly IWorkShiftManagementService _worshiftmanagement;
        private readonly IEmployeeScheduleManagementService _employeeSchedule;
        private readonly IShiftManagementService _shiftManagement;
        private readonly IVoucherManagementService _vouchermanagement;

        public UserLoginController(IAuthenticate userRepo, IWorkShiftManagementService workShiftManagementService, IEmployeeScheduleManagementService employeeScheduleManagementService, IShiftManagementService shiftManagementService, IVoucherManagementService voucherManagementService)
        {
            _userrepo = userRepo;
            _worshiftmanagement = workShiftManagementService;
            _employeeSchedule = employeeScheduleManagementService;
            _shiftManagement = shiftManagementService;
            _vouchermanagement = voucherManagementService;
        }
        //api đăng nhập tài khoản
        [HttpPost("User-Login")]
        public async Task<ResponseData<string>> Login(UserLoginView user)
        {
            var result = await _userrepo.Login(user);
            return result;
        }

        //api đăng ký tài khoản
        [Authorize(Roles = "Admin")]

        [HttpPost("User-Register")]
        public async Task<ResponseData<string>> Register(UserRegisterView user)
        {
            var result = await _userrepo.Register(user);
            return result;
        }

        //api quên mật khẩu
        [HttpGet("User-ForgotPass")]
        public async Task<ResponseMail> ForgotPassword(string mail)
        {
            var result = await _userrepo.ForgotPassword(mail);
            return result;
        }

        //change password
        [HttpPatch("changePassword")]
        public async Task<ResponseData<string>> ChangePassword(UserChangePasswordView changePasswordView)
        {
            var result = await _userrepo.ChangePassword(changePasswordView);
            return result;
        }

        //api lấy danh sách tài khoản nhân viên
        [Authorize(Roles = "Admin")]
        [HttpGet("List-User")]
        public async Task<ResponseData<List<UserInfView>>> GetUsers()
        {
            var result = await _userrepo.GetUsers();
            return result;
        }

        //api lấy số điện thoại nhân viên
        [Authorize(Roles = "Admin")]
        [HttpGet("User-Phone")]
        public async Task<ResponseData<UserInfView>> GetUserByPhoneNumber(string phonenumber)
        {
            var result = await _userrepo.GetUserAtPhoneNumber(phonenumber);
            return result;
        }

        //api thêm ca cho nhân viên
        [Authorize(Roles = "Admin")]
        [HttpGet("them-ca-for-all-staff")]
        public async Task<ResponseData<string>> Test()
        {
            var result = await _worshiftmanagement.InsertWorkShiftNextMonthCompareCurrentMonth();
            return result;
        }

        [Authorize(Roles = "Admin")]
        //api thêm ca cho nhiều nhân viên
        [HttpPost("them-ca-one-staff")]
        public async Task<ResponseData<string>> Test1(List<string> listUser, int shift)
        {
            var result = await _employeeSchedule.InsertEmployeeNextMonthCompareCurrentMonth(listUser, shift);
            return result;
        }

        //api tìm ca theo tháng năm
        [Authorize(Roles = "Admin")]
        [HttpGet("tim_ca_theo_thang_nam")]
        public async Task<ResponseData<List<ScheduleView>>> Test2(int month, int year)
        {
            var result = await _employeeSchedule.GetUsersInOneMonth(month, year);
            return result;
        }
        //api lấy ca theo id
        [Authorize(Roles = "Admin")]
        [HttpGet("find-ca-by-id")]
        public async Task<ResponseData<List<ScheduleView>>> GetAllCa(int ca)
        {
            var result = await _employeeSchedule.GetScheduleForShift(ca);
            return result;
        }

        //api lấy tất cả danh sách lịch làm việc của nhân viên
        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-ca-lam")]
        public async Task<ResponseData<List<ScheduleView>>> GetAll()
        {
            var result = await _employeeSchedule.GetAll();
            return result;
        }
        //api lấy ca làm theo tháng
        [Authorize(Roles = "Admin")]
        [HttpPost("get-month-to-month")]
        public async Task<ResponseData<List<ScheduleView>>> GetMonthToMonth(ScheduleMonthToMonthView view)
        {
            var result = await _employeeSchedule.GetScheduleFromMonthToMonth(view);
            return result;
        }

        [HttpPost("create-schedule-oneday-suddenly")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseData<string>> InsertOneDayScheduleForStaffSuddenly(List<string> listUser, int shift, DateTime dateTime)
        {
            var result = await _employeeSchedule.InsertOneDayScheduleForStaffSuddenly(listUser, shift, dateTime);
            return result;
        }

        //api xoá chuyển trạng thái của nhân viên
        [Authorize(Roles = "Admin")]
        [HttpGet("remove")]
        public async Task<ResponseData<string>> RemoveEmployee(string id)
        {
            var result = await _userrepo.RemoveUser(id);
            return result;
        }
        //api lấy nhân viên theo id
        [Authorize(Roles = "Admin")]
        [HttpGet("Get-id-user")]
        public Task<ResponseData<string>> GetId(string username)
        {
            var id = _userrepo.GetIdUser(username);
            return id;
        }

        //api lấy role của nhân viên
        [Authorize(Roles = "Admin")]
        [HttpGet("Get-role-user")]
        public async Task<ResponseData<string>> GetRoleUser(string id)
        {
            return await _userrepo.GetRoleUser(id);
        }

        //api thêm role cho nhân viên
        [Authorize(Roles = "Admin")]
        [HttpPost("Add-role-user")]
        public async Task<ResponseData<string>> AddRoleForUser(AddRoleForUserView addRoleForUserView)
        {
            return await _userrepo.AddRoleForUser(addRoleForUserView);
        }

        //api lấy chức vụ
        [Authorize(Roles = "Admin")]
        [HttpGet("List-Position")]
        public async Task<ResponseData<List<RoleView>>> ListPosition()
        {
            return await _userrepo.ListPosition();
        }

        //api tạo role
        [Authorize(Roles = "Admin")]
        [HttpPost("create-role")]
        public async Task<ResponseData<string>> AddRole(string roleName)
        {
            return await _userrepo.AddRole(roleName);
        }
        //api cập nhật nhân viên
        [Authorize(Roles = "Admin")]
        [HttpPatch("Update-inf")]
        public Task<ResponseData<string>> UpdateInfUser(UserUpdateView userUpdateView, string id)
        {
            return _userrepo.UpdateInformationUser(userUpdateView, id);
        }

        //api tạo mới ca làm
        [Authorize(Roles = "Admin")]
        [HttpPost("Create-shift")]
        public async Task<ResponseData<string>> CreateShift(ShiftView shift)
        {
            return await _shiftManagement.CreateShift(shift);
        }

        //api cập nhật ca làm
        [Authorize(Roles = "Admin")]
        [HttpPatch("Update-shift")]
        public async Task<ResponseData<string>> UpdateShift(ShiftView shift, int id)
        {
            return await _shiftManagement.UpdateShift(shift, id);
        }

        //api lấy ca làm
        [Authorize(Roles = "Admin")]
        [HttpGet("Get-List-Shift")]
        public async Task<ResponseData<List<Shift>>> GetListShift()
        {
            return await _shiftManagement.GetListShift();
        }

        //api acitve lại trạng thái tài khoản từ ngừng hoạt động sang hoạt dộng
        [Authorize(Roles = "Admin")]
        [HttpGet("Active-user")]
        public async Task<ResponseData<string>> ActiveUser(string id)
        {
            return await _userrepo.ActiveAccount(id);
        }
        [Authorize(Roles = "Admin,Receptionist,ServiceStaff")]
        [HttpGet("Get-user-inf-by-token/{id}")]
        public async Task<ResponseData<UserInfView>> GetInfByToken(string id)
        {
            return await _userrepo.GetInfById(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Create-Voucher")]
        public async Task<ResponseData<string>> CreateVoucher(VoucherView voucherView)
        {
            return await _vouchermanagement.CreateVoucher(voucherView);
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("Update-Voucher")]
        public async Task<ResponseData<string>> UpdateVoucher(VoucherView voucherView)
        {
            return await _vouchermanagement.UpdateVoucher(voucherView);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("List-Voucher")]
        public async Task<ResponseData<List<VoucherView>>> ListVoucher()
        {
            return await _vouchermanagement.GetAllVoucher();
        }

        [HttpPost("Reset-Pass")]
        public async Task<ResponseData<string>> ResetPass(UserResetPassView user)
        {
            return await _userrepo.ResetPassword(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Create-WorkShift-For-CurrentMonth")]
        public async Task<ResponseData<string>> CreateShiftForCurrentMonth()
        {
            return await _worshiftmanagement.InsertWorkShiftCurrentMonth();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Create-Shift-For-staff-in-CurrentMonth")]
        public async Task<ResponseData<string>> InsertEmployeeCurrentMonth(List<string> listUser, int shift)
        {
            return await _employeeSchedule.InsertEmployeeCurrentMonth(listUser, shift);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Get-List-Staff-Work-in-Day")]
        public async Task<ResponseData<List<NumberOfScheduleView>>> GetListStaffInDay(int shift, DateTime workdate)
        {
            return await _employeeSchedule.GetListStaffInDay(shift, workdate);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Get-List-Staff-Not-Working-in-Day")]
        public async Task<ResponseData<List<UserInfView>>> ListStaffNotWorkingInDay(int shiftId, DateTime workDate, string role)
        {
            return await _employeeSchedule.ListStaffNotWorkingInDay(shiftId, workDate, role);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Change-Shift")]
        public async Task<ResponseData<string>> ChangeShiftStaffToStaff(ChangeShiftView changeShiftView)
        {
            return await _employeeSchedule.ChangeShiftStaffToStaff(changeShiftView);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Change-Status-Voucher")]
        public async Task<ResponseData<string>> ChangeStatusVoucher(int id)
        {
            return await _vouchermanagement.ExpiresVoucher(id);
        }
        [HttpGet("Check-Otp")]
        public async Task<ResponseData<string>> CheckCode(string username, string code)
        {
            return await _userrepo.CheckCodeConfirm(username, code);
        }
        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet("Get-inf-by-token")]
        public async Task<ResponseData<string>> GetUserByToken(string token)
        {
            return await _userrepo.GetUserByToken(token);
        }
        [HttpGet("Get-all-voucher-can-apply")]
        public async Task<ResponseData<List<VoucherView>>> GetAllVoucherCanApply(double totalPrice)
        {
            return await _vouchermanagement.GetAllVoucherCanApply(totalPrice);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("update-url")]
        public async Task<ResponseData<string>> UpdateUrl(string url, string imgId, string id)
        {
            return await _userrepo.UpdateImg(url, imgId, id);
        }
    }
}
