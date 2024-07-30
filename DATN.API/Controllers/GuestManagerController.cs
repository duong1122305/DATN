using DATN.Aplication.Services;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestManagerController : ControllerBase
    {
        private readonly IGuestManagerService _guestManagerService;

        public GuestManagerController(IGuestManagerService guestManagerService)
        {
            _guestManagerService = guestManagerService;
        }
        [HttpPost("register-guest-with-user")]
        public async Task<ResponseData<string>> RegisterWithUser(GuestRegisterUserRequest request)
        {
            return await _guestManagerService.RegisterWithUser(request);
        }


        [HttpGet("GetGuest")]
        public async Task<ResponseData<List<GuestViewModel>>> GetGuestPaging()
        {
            var response = await _guestManagerService.GetGuest();
            return response;
        }
        [HttpGet("find-by-id")]
        public async Task<ResponseData<GuestViewModel>> FindByID(Guid id)
        {
            return await _guestManagerService.FindGuestByID(id);
        }
        [HttpGet("verify-cus")]//////////////đây là để xác minh khi đăng ký
        public async Task<ResponseData<string>> VerifyCode(string verifyCode)
        {
            return await _guestManagerService.VerififyUser(verifyCode);
        }
        [HttpPost("update-guest")]
        public async Task<ResponseData<string>> UpdateGuest(GuestUpdateRequest request)
        {
            return await _guestManagerService.UpdateGuest(request);
        }
        [HttpPost("change-status")]
        public async Task<ResponseData<string>> ChangStatus(DeleteRequest<Guid> request)
        {
            return await _guestManagerService.SoftDelete(request);
        }
        [HttpPost("update-pass-by-verifyCode")]
        public async Task<ResponseData<string>> UpdatePassByCode(string verifyCode, string newPass)
        {
            return await _guestManagerService.ChangPassWithVerifyCode(verifyCode, newPass);
        }
        [HttpPost("forgot-pass")]
        public async Task<ResponseData<string>> SendForgotMail(string email)
        {
            return await _guestManagerService.SendForgotMail(email);
        }

        [HttpPost("register-by-guest")]///////////////// đây là đăng ký mới
        public async Task<ResponseData<string>> RegisterByCustomer(GuestRegisterByGuestRequest request)
        {
            return await _guestManagerService.RegisterByCustomer(request);
        }

    }
}
