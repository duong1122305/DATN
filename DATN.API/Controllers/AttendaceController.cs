﻿using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendaceController : ControllerBase
    {
        private readonly IAttendanteMangarService _attendante;

        public AttendaceController(IAttendanteMangarService attendante)
        {
            _attendante = attendante;
        }
        [HttpGet]  
        public async Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID)
        {
            return await _attendante.ListAttendanceToday(shiftID);
        } 
        [HttpGet("select_shift-data")]  
        public async Task<ResponseData<List<Shift>>> GetShiftNow()
        {
            return await _attendante.GetShiftNow();
        }
    }
}