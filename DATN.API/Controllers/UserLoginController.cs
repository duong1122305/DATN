using DATN.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using DATN.Aplication.System;
using Azure;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.Aplication;
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

        public UserLoginController(IAuthenticate userRepo, IWorkShiftManagementService workShiftManagementService, IEmployeeScheduleManagementService employeeScheduleManagementService, IShiftManagementService shiftManagementService)
        {
            _userrepo = userRepo;
            _worshiftmanagement = workShiftManagementService;
            _employeeSchedule = employeeScheduleManagementService;
            _shiftManagement = shiftManagementService;
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
        [HttpPost("User-ForgotPass")]
        public async Task<ResponseMail> ForgotPassword(string mail)
        {
            var result = await _userrepo.ForgotPassword(mail);
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
        [HttpGet("test_them_ca_cho_1_thang")]
        public async Task<ResponseData<string>> Test()
        {
            var result = await _worshiftmanagement.InsertWorkShiftNextMonthCompareCurrentMonth();
            return result;
        }

        //api thêm ca cho nhân vêin
        [HttpPost("test_them_ca_cho_nhanvien_1thang")]
        public async Task<ResponseData<string>> Test1(List<string> listUser, int shift)
        {
            var result = await _employeeSchedule.InsertEmployeeNextMonthCompareCurrentMonth(listUser, shift);
            return result;
        }

        //api tìm ca theo tháng năm
        [HttpGet("tim_ca_theo_thang_nam")]
        public async Task<ResponseData<List<ScheduleView>>> Test2(int month, int year)
        {
            var result = await _employeeSchedule.GetUserInOneMonth(month, year);
            return result;
        }
        //api lấy ca theo id
        [HttpGet("get-all-1-ca")]
        public async Task<ResponseData<List<ScheduleView>>> GetAllCa(int ca)
        {
            var result = await _employeeSchedule.GetScheduleForShift(ca);
            return result;
        }

        //api lấy tất cả danh sách lịch làm việc của nhân viên
        [HttpGet("get-all")]
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

        //api acitve lại trạng thái tài khoản từ xoá sang hoạt dộng
        [HttpGet("Active-user")]
        public async Task<ResponseData<string>> ActiveUser(string id)
        {
            return await _userrepo.ActiveAccount(id);
        }
    }
}
