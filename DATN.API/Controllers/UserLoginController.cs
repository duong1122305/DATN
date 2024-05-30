using DATN.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using DATN.Aplication.Repositories;
using DATN.ViewModels;
using DATN.ViewModels.ViewModel;
using Azure;
using DATN.ViewModels.Common;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IUserRepo _userrepo;

        public UserLoginController(IUserRepo userRepo)
        {
            _userrepo = userRepo;
        }
        [HttpPost("User-Login")]
        public async Task<string> Login(UserLoginView user)
        {
            var result = await _userrepo.Login(user);
            return result;
        }

        [HttpPost("User-Register")]
        public async Task<string> Register(UserRegisterView user)
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
        public async Task<List<UserInfView>> GetUsers()
        {
            var result = await _userrepo.GetUsers();
            return result;
        }

        [HttpGet("User-Phone")]
        public async Task<User> GetUserByPhoneNumber(string phonenumber)
        {
            var result = await _userrepo.GetUserAtPhoneNumber(phonenumber);
            return result;
        }
    }
}
