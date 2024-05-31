﻿using DATN.Data.Entities;
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
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.System
{
    public class Authenticate : IAuthenticate
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly MailExtention _mail;
        private readonly RandomCodeExtention _random;
        private User _user;
        public Authenticate(UserManager<User> userManager, IConfiguration configuration, MailExtention mailExtention, RandomCodeExtention randomCodeExtention)
        {
            _userManager = userManager;
            _config = configuration;
            _mail = mailExtention;
            _random = randomCodeExtention;
        }
        public async Task<ResponseData<string>> Login(UserLoginView userView)
        {
            try
            {

                var userIdentity = await CheckUser(userView.UserName);
                if (userIdentity == null)
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Error="Tài khoản hoặc mật khẩu không chính xác"
                    };
                else
                {
                    if (await _userManager.CheckPasswordAsync(userIdentity, userView.Password))
                    {
                       
                        _user = userIdentity;
                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Data = await GenerateTokenString(userView)
                        };
                    }
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Error = "Tài khoản hoặc mật khẩu không chính xác"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = ex.Message
                };
               
            }

        }

        public async Task<string> GenerateTokenString(UserLoginView userViewModel)
        {
            var user = await CheckUser(userViewModel.UserName);
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
                FullName= userRegisterView.FullName,
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

        public async Task<ResponseMail> ForgotPassword(string username)
        {
            var userIdentity = await CheckUser(username);
            if (userIdentity == null) return new ResponseMail { IsSuccess = false, Notifications = "Tài khoản bạn nhập chưa được đăng ký!" };
            else
            {
                string newcode = _random.RandomCode();
                if (userIdentity != null)
                {
                    userIdentity.CodeConfirm = newcode;
                    var updateUser = await _userManager.UpdateAsync(userIdentity);
                    if (updateUser.Succeeded)
                        return await _mail.SendMailCodeForgot(userIdentity.Email, newcode);
                    else
                    {
                        return new ResponseMail { IsSuccess = false, Notifications = "CodeConfirm chưa được đưa vào database!!" };
                    }
                }
                else if (userIdentity != null)
                {
                    userIdentity.CodeConfirm = newcode;
                    var updateUser = await _userManager.UpdateAsync(userIdentity);
                    if (updateUser.Succeeded)
                        return await _mail.SendMailCodeForgot(userIdentity.Email, newcode);
                    else
                    {
                        return new ResponseMail { IsSuccess = false, Notifications = "CodeConfirm chưa được đưa vào database!!" };

                    }
                }
                else
                {
                    userIdentity.CodeConfirm = newcode;
                    var updateUser = await _userManager.UpdateAsync(userIdentity);
                    if (updateUser.Succeeded)
                        return await _mail.SendMailCodeForgot(userIdentity.Email, newcode);
                    else
                    {
                        return new ResponseMail { IsSuccess = false, Notifications = "CodeConfirm chưa được đưa vào database!!" };

                    }
                }
            }
        }

        public async Task<List<UserInfView>> GetUsers()
        {
            var listUserIdentity = await _userManager.Users.ToListAsync();
            var listUser = new List<UserInfView>();

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
            var userIdentity = await CheckUser(user.UserName);
            if (userIdentity == null) return "Tài khoản chưa được đăng kí";
            else
            {
                if (user.NewPassword == user.ConfirmPassword)
                {
                    var result = await _userManager.ChangePasswordAsync(userIdentity, user.OldPassword, user.NewPassword);
                    if (result.Succeeded)
                    {
                        return "Đổi mật khẩu thành công!!";
                    }
                    else return "Mật khẩu hiện tại đang sai hoặc mật khẩu mới chưa đúng định dạng";
                }
                else return "Mật khẩu mới và xác nhận lại mật khẩu mới không trùng khớp";
            }
        }

        public async Task<string> GetConfirmCode(string username)
        {
            var user = await CheckUser(username);
            if (user == null) return "Tài khoản nhập chưa được đăng kí";
            else
            {
                if (user != null)
                {
                    return user.CodeConfirm;
                }
                else if (user != null)
                {
                    return user.CodeConfirm;
                }
                else
                {
                    return user.CodeConfirm;
                }
            }
        }
        public async Task<bool> CheckCodeConfirm(string username, string code)
        {
            var user = await CheckUser(username);
            if (user != null)
            {
                if (code == user.CodeConfirm)
                {
                    user.CodeConfirm = null;
                    await _userManager.UpdateAsync(user);
                    return true;
                }
                return false;
            }
            return false;
        }
        private async Task<User> CheckUser(string username)
        {
            var userName = await _userManager.FindByNameAsync(username);
            var userEmail = await _userManager.FindByEmailAsync(username);
            var userPhone = await GetUserAtPhoneNumber(username);
            if (userPhone == null && userName == null && userEmail == null) return null;
            else
            {
                if (userEmail != null)
                {
                    return userEmail;
                }
                else if (userName != null)
                {
                    return userName;
                }
                else
                {
                    return userPhone;
                }
            }
        }
    }
}