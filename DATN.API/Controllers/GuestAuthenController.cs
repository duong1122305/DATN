﻿using DATN.Aplication.System;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GuestAuthenController : ControllerBase
	{
		private readonly IAuthenticateGuest _authenticateGuest;

		public GuestAuthenController(IAuthenticateGuest authenticateGuest)
        {
			_authenticateGuest = authenticateGuest;
		}
        [HttpPost("login")]
		public async Task<ResponseData<string>> Login(UserLoginView request)
		{
			return await _authenticateGuest.Login(request);	
		}

	}
}
