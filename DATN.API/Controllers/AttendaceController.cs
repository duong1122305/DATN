using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.Utilites;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    [Authorize(Roles = "Admin,Receptionist")]
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
        [HttpGet("check-in-hand")]
        public async Task<ResponseData<string>> CheckInHand(int scheduleId, int attendanceID, bool isCheckin, Guid userId)
        {
            return await _attendante.CheckIn(scheduleId, attendanceID, isCheckin, userId);
        }
        [HttpGet("check-out-hand")]
        public async Task<ResponseData<string>> CheckOutHand(int scheduleId, int attendanceID, bool isCheckout, Guid userId)
        {
            return await _attendante.CheckOut(scheduleId, attendanceID, isCheckout, userId);
        }
        [HttpGet("check-attendance-personal")]
        public async Task<ResponseData<AttendancePersonal>> GetShiftQR(Guid id)
        {
            return await _attendante.GetPersonalShift(id);
        }
        [HttpPost("check-in-per")]
        public async Task<ResponseData<string>> CheckInPer(AttendancePersonal attendance)
        {
            return await _attendante.CheckInPersonal(attendance);
        }
        [HttpPost("check-out-per")]
        public async Task<ResponseData<string>> CheckOutPer(AttendancePersonal attendance)
        {
            return await _attendante.CheckOutPersonal(attendance);
        }
        [HttpGet("GetAllAttendance")]
        public async Task<ResponseData<List<AttendanceMonth>>> GetAllAttandanceMonth(int month = 0, int year = 0, int isDelete = -1)
        {
            return await _attendante.GetAllAttandanceMonth(month, year, isDelete);
        }
        [HttpGet("check-shift")]
        public async Task<ResponseData<string>> CheckShift(int shiftID, bool isCheckin)
        {
            return await _attendante.CheckLate(shiftID, isCheckin);
        }
        [HttpGet("get-attendance-month-by-user")]
        public async Task<ResponseData<ListPerAttenMonth>> GetAttandanceMonth(Guid idUser, int month = 0, int year = 0, int typeAttendance = Contant.AllAttendance)
        {
            return await _attendante.GetAttandanceMonth(idUser, month, year, typeAttendance);
        }



    }
}
