using DATN.Aplication.Services;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;
using Microsoft.AspNetCore.Http;
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
        [HttpPost("register-guest-no-user")]
        public async Task<ResponseData<string>> RegisterNoUser(GuestRegisterNoUserRequest request)
        {
            return await _guestManagerService.RegisterNoUser(request);
        }
    }
}
