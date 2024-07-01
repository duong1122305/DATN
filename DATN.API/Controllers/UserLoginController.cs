using DATN.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using DATN.Aplication.System;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.Aplication.Services.IServices;

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
        [HttpPut("changePassword")]
        public async Task<ResponseData<string>> ChangePassword(UserChangePasswordView changePasswordView)
        {
            var result = await _userrepo.ChangePassword(changePasswordView);
            return result;
        }

        //api lấy danh sách tài khoản nhân viên
        [HttpGet("List-User")]
        public async Task<ResponseData<List<UserInfView>>> GetUsers()
        {
            var result = await _userrepo.GetUsers();
            return result;
        }

        //api lấy số điện thoại nhân viên
        [HttpGet("User-Phone")]
        public async Task<ResponseData<UserInfView>> GetUserByPhoneNumber(string phonenumber)
        {
            var result = await _userrepo.GetUserAtPhoneNumber(phonenumber);
            return result;
        }

        //api thêm ca cho nhân viên
        [HttpGet("them-ca-for-all-staff")]
        public async Task<ResponseData<string>> Test()
        {
            var result = await _worshiftmanagement.InsertWorkShiftNextMonthCompareCurrentMonth();
            return result;
        }

        //api thêm ca cho nhiều nhân viên
        [HttpPost("them-ca-one-staff")]
        public async Task<ResponseData<string>> Test1(List<string> listUser, int shift)
        {
            var result = await _employeeSchedule.InsertEmployeeNextMonthCompareCurrentMonth(listUser, shift);
            return result;
        }

        //api tìm ca theo tháng năm
        [HttpGet("tim_ca_theo_thang_nam")]
        public async Task<ResponseData<List<ScheduleView>>> Test2(int month, int year)
        {
            var result = await _employeeSchedule.GetUsersInOneMonth(month, year);
            return result;
        }
        //api lấy ca theo id
        [HttpGet("find-ca-by-id")]
        public async Task<ResponseData<List<ScheduleView>>> GetAllCa(int ca)
        {
            var result = await _employeeSchedule.GetScheduleForShift(ca);
            return result;
        }

        //api lấy tất cả danh sách lịch làm việc của nhân viên
        [HttpGet("get-all-ca-lam")]
        public async Task<ResponseData<List<ScheduleView>>> GetAll()
        {
            var result = await _employeeSchedule.GetAll();
            return result;
        }
        //api lấy ca làm theo tháng
        [HttpPost("get-month-to-month")]
        public async Task<ResponseData<List<ScheduleView>>> GetMonthToMonth(ScheduleMonthToMonthView view)
        {
            var result = await _employeeSchedule.GetScheduleFromMonthToMonth(view);
            return result;
        }

        //api xoá chuyển trạng thái của nhân viên
        [HttpGet("remove")]
        public async Task<ResponseData<string>> RemoveEmployee(string id)
        {
            var result = await _userrepo.RemoveUser(id);
            return result;
        }
        //api lấy nhân viên theo id
        [HttpGet("Get-id-user")]
        public Task<ResponseData<string>> GetId(string username)
        {
            var id = _userrepo.GetIdUser(username);
            return id;
        }

        //api lấy role của nhân viên
        [HttpGet("Get-role-user")]
        public async Task<ResponseData<string>> GetRoleUser(string id)
        {
            return await _userrepo.GetRoleUser(id);
        }

        //api thêm role cho nhân viên
        [HttpPost("Add-role-user")]
        public async Task<ResponseData<string>> AddRoleForUser(AddRoleForUserView addRoleForUserView)
        {
            return await _userrepo.AddRoleForUser(addRoleForUserView);
        }

        //api lấy chức vụ
        [HttpGet("List-Position")]
        public async Task<ResponseData<List<string>>> ListPosition()
        {
            return await _userrepo.ListPosition();
        }

        //api tạo role
        [HttpPost("create-role")]
        public async Task<ResponseData<string>> AddRole(string roleName)
        {
            return await _userrepo.AddRole(roleName);
        }
        //api cập nhật nhân viên
        [HttpPut("Update-inf")]
        public Task<ResponseData<string>> UpdateInfUser(UserUpdateView userUpdateView, string id)
        {
            return _userrepo.UpdateInformationUser(userUpdateView, id);
        }

        //api tạo mới ca làm
        [HttpPost("Create-shift")]
        public async Task<ResponseData<string>> CreateShift(ShiftView shift)
        {
            return await _shiftManagement.CreateShift(shift);
        }

        //api cập nhật ca làm
        [HttpPut("Update-shift")]
        public async Task<ResponseData<string>> UpdateShift(ShiftView shift, int id)
        {
            return await _shiftManagement.UpdateShift(shift, id);
        }

        //api lấy ca làm
        [HttpGet("Get-List-Shift")]
        public async Task<ResponseData<List<Shift>>> GetListShift()
        {
            return await _shiftManagement.GetListShift();
        }

        //api acitve lại trạng thái tài khoản từ ngừng hoạt động sang hoạt dộng
        [HttpGet("Active-user")]
        public async Task<ResponseData<string>> ActiveUser(string id)
        {
            return await _userrepo.ActiveAccount(id);
        }
        [HttpGet("Get-user-inf-by-token/{id}")]
        public async Task<ResponseData<UserInfView>> GetInfByToken(string id)
        {
            return await _userrepo.GetInfByToken(id);
        }
        [HttpPost("Create-Voucher")]
        public async Task<ResponseData<string>> CreateVoucher(VoucherView voucherView)
        {
            return await _vouchermanagement.CreateVoucher(voucherView);
        }

        [HttpPut("Update-Voucher")]
        public async Task<ResponseData<string>> UpdateVoucher(VoucherView voucherView)
        {
            return await _vouchermanagement.UpdateVoucher(voucherView);
        }
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
        [HttpGet("Create-WorkShift-For-CurrentMonth")]
        public async Task<ResponseData<string>> CreateShiftForCurrentMonth()
        {
            return await _worshiftmanagement.InsertWorkShiftCurrentMonth();
        }

        [HttpPost("Create-Shift-For-staff-in-CurrentMonth")]
        public async Task<ResponseData<string>> InsertEmployeeCurrentMonth(List<string> listUser, int shift)
        {
            return await _employeeSchedule.InsertEmployeeCurrentMonth(listUser, shift);
        }

        [HttpGet("Get-List-Staff-Work-in-Day")]
        public async Task<ResponseData<List<NumberOfScheduleView>>> GetListStaffInDay(int shift, DateTime workdate)
        {
            return await _employeeSchedule.GetListStaffInDay(shift, workdate);
        }
        [HttpGet("Get-List-Staff-Not-Working-in-Day")]
        public async Task<ResponseData<List<UserInfView>>> ListStaffNotWorkingInDay(int shiftId, DateTime workDate)
        {
            return await _employeeSchedule.ListStaffNotWorkingInDay(shiftId, workDate);
        }
        [HttpPost("Change-Shift")]
        public async Task<ResponseData<string>> ChangeShiftStaffToStaff(ChangeShiftView changeShiftView)
        {
            return await _employeeSchedule.ChangeShiftStaffToStaff(changeShiftView);
        }
        [HttpGet("Change-Status-Voucher")]
        public async Task<ResponseData<string>> ChangeStatusVoucher(int id)
        {
            return await _vouchermanagement.ExpiresVoucher(id);
        }
        [HttpGet("Check-Otp")]
        public async Task<ResponseData<string>> CheckCode(string username,string code)
        {
            return await _userrepo.CheckCodeConfirm(username,code);
        }
    }
}
