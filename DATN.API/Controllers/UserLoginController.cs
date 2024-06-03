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

        public UserLoginController(IAuthenticate userRepo, IWorkShiftManagementService workShiftManagementService, IEmployeeScheduleManagementService employeeScheduleManagementService)
        {
            _userrepo = userRepo;
            _worshiftmanagement = workShiftManagementService;
            _employeeSchedule = employeeScheduleManagementService;
        }
        [HttpPost("User-Login")]
        public async Task<ResponseData<string>> Login(UserLoginView user)
        {
            var result = await _userrepo.Login(user);
            return result;
        }

        [HttpPost("User-Register")]
        public async Task<ResponseData<string>> Register(UserRegisterView user)
        {
            var result = await _userrepo.Register(user);
            return result;
        }
        [HttpPost("User-ForgotPass")]
        public async Task<ResponseMail> ForgotPassword(string mail)
        {
            var result = await _userrepo.ForgotPassword(mail);
            return result;
        }

        [HttpGet("List-User")]
        public async Task<ResponseData<List<UserInfView>>> GetUsers()
        {
            var result = await _userrepo.GetUsers();
            return result;
        }

        [HttpGet("User-Phone")]
        public async Task<ResponseData<UserInfView>> GetUserByPhoneNumber(string phonenumber)
        {
            var result = await _userrepo.GetUserAtPhoneNumber(phonenumber);
            return result;
        }

        [HttpPost("test_them_ca_cho_1_thang")]
        public async Task<ResponseData<string>> Test()
        {
            var result = await _worshiftmanagement.InsertWorkShiftNextMonthCompareCurrentMonth();
            return result;
        }
        [HttpPost("test_them_ca_cho_nhanvien_1thang")]
        public async Task<ResponseData<string>> Test1(List<string> listUser, int shift)
        {
            var result = await _employeeSchedule.InsertEmployeeNextMonthCompareCurrentMonth(listUser, shift);
            return result;
        }
        [HttpGet("tim_ca_theo_thang_nam")]
        public async Task<ResponseData<List<ScheduleView>>> Test2(int month, int year)
        {
            var result = await _employeeSchedule.GetUserInOneMonth(month, year);
            return result;
        }
        [HttpGet("get-all-1-ca")]
        public async Task<ResponseData<List<ScheduleView>>> GetAllCa(int ca)
        {
            var result = await _employeeSchedule.GetScheduleForShift(ca);
            return result;
        }
        [HttpGet("get-all")]
        public async Task<ResponseData<List<ScheduleView>>> GetAll()
        {
            var result = await _employeeSchedule.GetAll();
            return result;
        }
        [HttpPost("get-month-to-month")]
        public async Task<ResponseData<List<ScheduleView>>> GetMonthToMonth(ScheduleMonthToMonthView view)
        {
            var result = await _employeeSchedule.GetScheduleFromMonthToMonth(view);
            return result;
        }
        [HttpPut("remove")]
        public async Task<ResponseData<string>> RemoveEmployee(string id)
        {
            var result = await _userrepo.RemoveUser(id);
            return result;
        }
        [HttpGet("Get-id-user")]
        public Task<ResponseData<string>> GetId(string username)
        {
            var id = _userrepo.GetIdUser(username);
            return id;
        }
        [HttpGet("Get-role-user")]
        public async Task<ResponseData<string>> GetRoleUser(string id)
        {
            return await _userrepo.GetRoleUser(id);
        }
        [HttpPost("Add-role-user")]
        public async Task<ResponseData<string>> AddRoleForUser(AddRoleForUserView addRoleForUserView)
        {
            return await _userrepo.AddRoleForUser(addRoleForUserView);
        }
        [HttpGet("List-Position")]
        public async Task<ResponseData<List<string>>> ListPosition()
        {
            return await _userrepo.ListPosition();
        }
        [HttpPost("create-role")]
        public async Task<ResponseData<string>> AddRole(string roleName)
        {
            return await _userrepo.AddRole(roleName);
        }

    }
}
