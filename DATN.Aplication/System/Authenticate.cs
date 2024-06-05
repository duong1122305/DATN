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
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _config;
        private readonly MailExtention _mail;
        private readonly RandomCodeExtention _random;
        private User _user;
        public Authenticate(UserManager<User> userManager, IConfiguration configuration, MailExtention mailExtention, RandomCodeExtention randomCodeExtention, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _config = configuration;
            _mail = mailExtention;
            _random = randomCodeExtention;
            _roleManager = roleManager;
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
                        Error = "Tài khoản hoặc mật khẩu không chính xác"
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

        private async Task<string> GenerateTokenString(UserLoginView userViewModel)
        {
            var user = await CheckUser(userViewModel.UserName);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, string.Join(",",await _userManager.GetRolesAsync(_user))),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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

        public async Task<ResponseData<string>> Register(UserRegisterView userRegisterView)
        {
            var userIdentity = new User()
            {
                FullName = userRegisterView.FullName,
                UserName = userRegisterView.UserName,
                Email = userRegisterView.Email,
                PhoneNumber = userRegisterView.PhoneNumber,
                Address = userRegisterView.Address,
                NormalizedEmail = userRegisterView.Email.ToUpper(),
                NormalizedUserName = userRegisterView.UserName.ToUpper(),
                IsDeleted = false,
            };
            var checkEmail = await _userManager.FindByEmailAsync(userRegisterView.Email);
            var checkPhone = await GetUserAtPhoneNumber(userIdentity.PhoneNumber);
            if (checkEmail == null && checkPhone.Data == null)
            {
                var result = await _userManager.CreateAsync(userIdentity, userRegisterView.Password);
                if (result.Succeeded)
                {
                    var userInf = new UserLoginView()
                    {
                        UserName = userRegisterView.UserName,
                        Password = userRegisterView.Password
                    };
                    return new ResponseData<string> { IsSuccess = true, Data = await _mail.SendMailAccountStaffAsync(userRegisterView.Email, userInf) };
                }
                return new ResponseData<string> { IsSuccess = false, Error = "Tạo tài khoản không thành công" };
            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "Thông tin tài khoản bị trùng với thông tin tài khoản đã có( Email or PhoneNumber )!!" };
        }

        public async Task<ResponseMail> ForgotPassword(string username)
        {
            var userIdentity = await CheckUser(username);
            if (userIdentity == null) return new ResponseMail { IsSuccess = false, Notifications = "Tài khoản bạn nhập chưa được đăng ký!" };
            else
            {
                string newcode = _random.RandomCode();

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

        public async Task<ResponseData<List<UserInfView>>> GetUsers()
        {
            var listUserIdentity = await _userManager.Users.ToListAsync();
            var listUser = new List<UserInfView>();

            if (listUserIdentity.Count > 0)
            {
                foreach (var user in listUserIdentity)
                {
                    var userInfView = new UserInfView();
                    userInfView.Position = string.Join("", await _userManager.GetRolesAsync(user));
                    if (userInfView.Position != "Admin")
                    {
                        userInfView.FullName = user.FullName;
                        userInfView.UserName = user.UserName;
                        userInfView.Address = user.Address;
                        userInfView.Email = user.Email;
                        userInfView.PhoneNumber = user.PhoneNumber;
                        userInfView.IsDeleted = user.IsDeleted;
                        listUser.Add(userInfView);
                    }
                }
                return new ResponseData<List<UserInfView>> { IsSuccess = true, Data = listUser };
            }
            else
                return new ResponseData<List<UserInfView>> { IsSuccess = true, Error = "Ko có nhân viên nào trong danh sách" };

        }

        public async Task<ResponseData<UserInfView>> GetUserAtPhoneNumber(string phonenumber)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(c => c.PhoneNumber == phonenumber);
            if (user != null)
            {
                var userInfView = new UserInfView();
                userInfView.FullName = user.FullName;
                userInfView.Email = user.Email;
                userInfView.PhoneNumber = user.PhoneNumber;
                userInfView.Address = user.Address;
                userInfView.Position = string.Join("", await _userManager.GetRolesAsync(user));
                return new ResponseData<UserInfView> { IsSuccess = true, Data = userInfView };
            }
            else
                return new ResponseData<UserInfView> { IsSuccess = false, Error = "Chưa có khách hàng nào đăng kí trên số điện thoại này" };
        }

        public async Task<ResponseData<string>> UpdateInformationUser(UserUpdateView userUpdateView, string id)
        {
            var userIdentity = await _userManager.FindByIdAsync(id);
            if (userIdentity == null) return new ResponseData<string> { IsSuccess = false, Error = "Tài khoản nhập chưa được đăng kí" };
            else
            {
                userIdentity.FullName = userUpdateView.FullName;
                userIdentity.PhoneNumber = userUpdateView.PhoneNumber;
                userIdentity.Address = userUpdateView.Address;
                userIdentity.Email = userUpdateView.Email;

                var result = await _userManager.UpdateAsync(userIdentity);
                if (result.Succeeded)
                    return new ResponseData<string> { IsSuccess = result.Succeeded, Data = "Cập nhật thông tin tài khoản thành công!!" };
                else
                    return new ResponseData<string> { IsSuccess = result.Succeeded, Error = "Thông tin chưa được thay đổi" };
            }
        }

        public async Task<ResponseData<string>> ChangePassword(UserChangePasswordView user)
        {
            var userIdentity = await CheckUser(user.UserName);
            if (userIdentity == null) return new ResponseData<string> { IsSuccess = false, Error = "Tài khoản chưa có" };
            else
            {
                if (user.NewPassword == user.ConfirmPassword)
                {
                    var result = await _userManager.ChangePasswordAsync(userIdentity, user.OldPassword, user.NewPassword);
                    if (result.Succeeded)
                    {
                        return new ResponseData<string> { IsSuccess = true, Data = "Đổi mật khẩu thành công!!" };
                    }
                    else return new ResponseData<string> { IsSuccess = false, Error = "Mật khẩu hiện tại đang sai hoặc mật khẩu mới chưa đúng định dạng" };
                }
                else return new ResponseData<string> { IsSuccess = false, Error = "Mật khẩu mới và xác nhận lại mật khẩu mới không trùng khớp" };
            }
        }

        public async Task<ResponseData<string>> GetConfirmCode(string username)
        {
            var user = await CheckUser(username);
            if (user == null) return new ResponseData<string> { IsSuccess = true, Error = "Tài khoản nhập chưa được đăng kí" };
            else
            {
                return new ResponseData<string> { IsSuccess = true, Data = user.CodeConfirm };
            }
        }
        public async Task<ResponseData<bool>> CheckCodeConfirm(string username, string code)
        {
            var user = await CheckUser(username);
            if (user != null)
            {
                if (code == user.CodeConfirm)
                {
                    user.CodeConfirm = null;
                    await _userManager.UpdateAsync(user);
                    return new ResponseData<bool> { IsSuccess = true };
                }
                return new ResponseData<bool> { IsSuccess = false, Error = "Code chưa đúng" };
            }
            return new ResponseData<bool> { IsSuccess = false, Error = "Tài khoản hoặc mật khẩu chưa đúng!" };
        }
        private async Task<User> CheckUser(string username)
        {
            var userName = await _userManager.FindByNameAsync(username);
            var userEmail = await _userManager.FindByEmailAsync(username);
            var userPhone = await _userManager.Users.FirstOrDefaultAsync(c => c.PhoneNumber == username);
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

        public async Task<ResponseData<string>> RemoveUser(string id)
        {
            var userIdentity = await _userManager.FindByIdAsync(id);
            if (userIdentity != null)
            {
                userIdentity.IsDeleted = true;
                await _userManager.UpdateAsync(userIdentity);
                return new ResponseData<string> { IsSuccess = true, Data = "Xóa user thành công!" };
            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "Xóa lỗi" };
        }

        public async Task<ResponseData<string>> GetIdUser(string username)
        {
            var user = await CheckUser(username);
            if (user != null)
                return new ResponseData<string> { IsSuccess = true, Data = user.Id.ToString() };
            else
                return new ResponseData<string> { IsSuccess = false, Error = "Không có user này" };
        }
        public async Task<ResponseData<string>> AddRoleForUser(AddRoleForUserView addRoleForUserView)
        {
            var queryRole = await _roleManager.FindByIdAsync(addRoleForUserView.RoleId);
            var user = await _userManager.FindByIdAsync(addRoleForUserView.UserId);
            if (queryRole != null)
            {
                var queryUser = await _userManager.AddToRoleAsync(user, queryRole.Name);
                if (queryUser.Succeeded)
                {
                    return new ResponseData<string> { IsSuccess = true, Data = "Thêm chức vụ cho người dùng thành công" };
                }
                return new ResponseData<string> { IsSuccess = false, Error = "User không có" };
            }
            return new ResponseData<string> { IsSuccess = false, Error = "Chức vụ không có" };
        }

        public async Task<ResponseData<List<string>>> ListPosition()
        {
            var listRole = await _roleManager.Roles.ToListAsync();
            if (listRole.Count > 0)
            {
                var list = new List<string>();
                foreach (var role in listRole)
                {
                    list.Add(role.Name);
                }
                return new ResponseData<List<string>> { IsSuccess = true, Data = list };
            }
            else
                return new ResponseData<List<string>> { IsSuccess = false, Error = "Chưa có chức vụ nào" };
        }

        public async Task<ResponseData<string>> GetRoleUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                if (role == null)
                    return new ResponseData<string> { IsSuccess = false, Error = "Người dùng chưa có chức vụ" };
                else
                    return new ResponseData<string> { IsSuccess = true, Data = string.Join(" ", role) };
            }
            return new ResponseData<string> { IsSuccess = false, Error = "Không có người dùng này" };
        }

        public async Task<ResponseData<string>> AddRole(string roleName)
        {
            var roleIdentity = new Role()
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            };
            var createRole = await _roleManager.CreateAsync(roleIdentity);
            if (createRole.Succeeded)
            {
                return new ResponseData<string> { IsSuccess = true, Data = "Thêm chức vụ mới thành công" };
            }
            return new ResponseData<string> { IsSuccess = false, Error = "Lỗi đéo biêts" };
        }
    }
}
