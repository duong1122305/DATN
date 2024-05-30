using DATN.ViewModels;
using DATN.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DATN.Aplication.Extentions;
using DATN.ViewModels.ViewModel;

namespace DATN.Aplication.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly MailExtention _mail;
        private readonly RandomCodeExtention _random;
        private User _user;
        public UserRepo(UserManager<User> userManager, IConfiguration configuration, MailExtention mailExtention, RandomCodeExtention randomCodeExtention)
        {
            _userManager = userManager;
            _config = configuration;
            _mail = mailExtention;
            _random = randomCodeExtention;
        }
        public async Task<string> Login(UserLoginView userView)
        {
            var userName = await _userManager.FindByNameAsync(userView.UserName);
            var userEmail = await _userManager.FindByEmailAsync(userView.UserName);
            var UserPhone = await GetUserAtPhoneNumber(userView.UserName);
            if (UserPhone == null && userName == null && userEmail == null) return null;
            else
            {
                if (userName != null)
                {
                    if (await _userManager.CheckPasswordAsync(userName, userView.Password))
                    {
                        _user = userName;
                        return await GenerateTokenString(userView);
                    }
                    return null;
                }
                else if (userEmail != null)
                {
                    if (await _userManager.CheckPasswordAsync(userEmail, userView.Password))
                    {
                        _user = userEmail;
                        return await GenerateTokenString(userView);
                    }
                    return null;
                }
                else
                {
                    if (await _userManager.CheckPasswordAsync(UserPhone, userView.Password))
                    {
                        _user = UserPhone;
                        return await GenerateTokenString(userView);
                    }
                    return null;
                }
            }

        }

        public async Task<string> GenerateTokenString(UserLoginView userViewModel)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userViewModel.UserName),
                new Claim(ClaimTypes.Role, string.Join(",",await _userManager.GetRolesAsync(_user)))
            };
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value));
            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            SecurityToken securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                issuer: _config.GetSection("JWT:Issuer").Value,
                audience: _config.GetSection("JWT:Audience").Value,
                signingCredentials: signingCred
                );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        public async Task<string> Register(UserRegisterView userRegisterView)
        {
            var userIdentity = new User()
            {
                UserName = userRegisterView.UserName,
                Email = userRegisterView.Email,
                PhoneNumber = userRegisterView.PhoneNumber,
                Address = userRegisterView.Address,
                NormalizedEmail = userRegisterView.Email.ToUpper(),
                NormalizedUserName = userRegisterView.UserName.ToUpper(),
            };
            var checkEmail = await _userManager.FindByEmailAsync(userRegisterView.Email);
            var checkPhone = await GetUserAtPhoneNumber(userIdentity.PhoneNumber);
            if (checkEmail == null && checkPhone == null)
            {
                var result = await _userManager.CreateAsync(userIdentity, userRegisterView.Password);
                if (result.Succeeded)
                {
                    var userInf = new UserLoginView()
                    {
                        UserName = userRegisterView.UserName,
                        Password = userRegisterView.Password
                    };
                    return await _mail.SendMailAccountStaffAsync(userRegisterView.Email, userInf);
                }
                return "Tạo tài khoản không thành công";
            }
            return "Thông tin tài khoản bị trùng với thông tin tài khoản đã có( Email or PhoneNumber )!!";
        }

        public async Task<string> ForgotPassword(string username)
        {
            var userName = await _userManager.FindByNameAsync(username);
            var userEmail = await _userManager.FindByEmailAsync(username);
            var UserPhone = await _userManager.Users.FirstOrDefaultAsync(c => c.PhoneNumber == username);
            if (UserPhone == null && userName == null && userEmail == null) return "Tài khoản của bạn chưa được đăng kí";
            else
            {
                string newcode = _random.RandomCode();
                if (userName != null)
                {
                    userName.Address = newcode;
                    var updateUser = await _userManager.UpdateAsync(userName);
                    if (updateUser.Succeeded)
                        return await _mail.SendMailCodeForgot(userName.Email, newcode);
                    else
                    {
                        return "Lỏ rồi code chưa được đưa vào db đâu nhá";
                    }
                }
                else if (userEmail != null)
                {
                    userEmail.Address = newcode;
                    var updateUser = await _userManager.UpdateAsync(userName);
                    if (updateUser.Succeeded)
                        return await _mail.SendMailCodeForgot(userEmail.Email, newcode);
                    else
                    {
                        return "Lỏ rồi code chưa được đưa vào db đâu nhá";
                    }
                }
                else
                {
                    UserPhone.Address = newcode;
                    var updateUser = await _userManager.UpdateAsync(userName);
                    if (updateUser.Succeeded)
                        return await _mail.SendMailCodeForgot(UserPhone.Email, newcode);
                    else
                    {
                        return "Lỏ rồi code chưa được đưa vào db đâu nhá";
                    }
                }
            }
        }

        public async Task<List<UserInfView>> GetUsers()
        {
            var listUserIdentity = await _userManager.Users.ToListAsync();
            var listUser=new List<UserInfView>();

            foreach (var user in listUserIdentity)
            {
                var userInfView = new UserInfView();
                userInfView.Position = string.Join("", await _userManager.GetRolesAsync(user));
                if (userInfView.Position != "Admin")
                {
                    userInfView.Name = user.FullName;
                    userInfView.Address = user.Address;
                    userInfView.Email = user.Email;
                    userInfView.PhoneNumber = user.PhoneNumber;
                    listUser.Add(userInfView); 
                }
            }
            return listUser;
        }

        public async Task<User> GetUserAtPhoneNumber(string phonenumber)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(c => c.PhoneNumber == phonenumber);
            return user;
        }

        public async Task<bool> UpdateInformationUser(UserRegisterView userRegisterView)
        {
            var userIdentity = await _userManager.FindByNameAsync(userRegisterView.UserName);
            if (userIdentity == null) return false;
            else
            {
                userIdentity.PhoneNumber = userRegisterView.PhoneNumber;
                userIdentity.Address = userRegisterView.Address;
                userIdentity.Email = userRegisterView.Email;

                var result = _userManager.UpdateAsync(userIdentity);
                return result.IsCompleted;
            }
        }

        public async Task<string> ChangePassword(UserChangePasswordView user)
        {
            var userName = await _userManager.FindByNameAsync(user.UserName);
            var userEmail = await _userManager.FindByEmailAsync(user.UserName);
            var UserPhone = await GetUserAtPhoneNumber(user.UserName);
            if (UserPhone == null && userName == null && userEmail == null) return "Tài khoản chưa được đăng kí";
            else
            {
                if (userName != null)
                {
                    _user = userName;
                }
                else if (userEmail != null)
                {
                    _user = userEmail;
                }
                else
                {
                    _user = UserPhone;
                }
                if (user.NewPassword == user.ConfirmPassword)
                {
                    var result = await _userManager.ChangePasswordAsync(_user, user.OldPassword, user.NewPassword);
                    if (result.Succeeded)
                    {
                        return "Đổi mật khẩu thành công!!";
                    }
                    else return "Mật khẩu hiện tại đang sai hoặc mật khẩu mới chưa đúng định dạng";
                }
                else return "Mật khẩu mới và xác nhận lại mật khẩu mới không trùng khớp";
            }
        }
    }
}
