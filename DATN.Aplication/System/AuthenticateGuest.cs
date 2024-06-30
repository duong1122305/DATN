using DATN.Aplication.Extentions;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.DTOs.Guest;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.System
{
	public class AuthenticateGuest : IAuthenticateGuest
	{
		private readonly IAuthenticate _authenticate;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		PasswordExtensitons _passwordExtensitons;

		public AuthenticateGuest(IAuthenticate authenticate, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
			_authenticate = authenticate;
			_unitOfWork = unitOfWork;
			_configuration = configuration;
			_passwordExtensitons = new PasswordExtensitons();
		}
        public async Task<ResponseData<string>> Login(UserLoginView request)
		{
			try
			{
                if (string.IsNullOrEmpty(request.UserName)|| string.IsNullOrEmpty(request.Password))
                {
					return  new ResponseData<string>(false, "Tên đăng nhập hoặc mật khẩu không chính xác");
                }

				var guest = await _unitOfWork.GuestRepository.FindAsync(x => (x.Email == request.UserName || x.UserName == request.UserName || x.PhoneNumber == request.UserName)
				&& x.PasswordHash == _passwordExtensitons.HashPassword(request.Password) && x.IsComfirm != false && x.IsDeleted != true);
				if (guest != null|| guest.Count()!=0)
				{

					return new ResponseData<string>( await GenerateTokenString(guest.First()));

				}
				return new ResponseData<string>(false, "Tên đăng nhập hoặc mật khẩu không chính xác");
			}
			catch (Exception ex)
			{

				return new ResponseData<string>(false, "Có lỗi xảy ra trong quá trình kết nối máy chủ");
			}
		}


		public Task<ResponseData<string>> Logout(string UserName)
		{
			throw new NotImplementedException();
		}
		private async Task<string> GenerateTokenString(Guest guest)
		{
			
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, guest.UserName!),
				new Claim(ClaimTypes.UserData, guest.Name),
				new Claim(ClaimTypes.NameIdentifier, guest.Id.ToString()),
				new Claim(ClaimTypes.Email, guest.Email!),
			};
			SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value));
			SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
			SecurityToken securityToken = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(30),
				issuer: _configuration.GetSection("JWT:Issuer").Value,
				audience: _configuration.GetSection("JWT:Audience").Value,
				signingCredentials: signingCred
				);
			string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
			return token;
		}
	}
}
