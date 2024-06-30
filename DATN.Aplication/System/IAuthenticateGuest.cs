using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.System
{
	public interface IAuthenticateGuest
	{
		Task<ResponseData<string>> Login(UserLoginView request);
	
		
	}
}
