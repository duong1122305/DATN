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
    public class AttendaceController : ControllerBase
    {
        private readonly IAttendanteMangarService _attendante;

        public AttendaceController(IAttendanteMangarService attendante)
        {
            _attendante = attendante;
        }
        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet]
        public async Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID)
        {
            return await _attendante.ListAttendanceToday(shiftID);
        }
        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet("select_shift-data")]
        public async Task<ResponseData<List<Shift>>> GetShiftNow()
        {
            return await _attendante.GetShiftNow();
        }
        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet("check-in-hand")]
        public async Task<ResponseData<string>> CheckInHand(int scheduleId, int attendanceID, bool isCheckin, Guid userId)
        {
            return await _attendante.CheckIn(scheduleId, attendanceID, isCheckin, userId);
        }
        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet("check-out-hand")]
        public async Task<ResponseData<string>> CheckOutHand(int scheduleId, int attendanceID, bool isCheckout, Guid userId)
        {
            return await _attendante.CheckOut(scheduleId, attendanceID, isCheckout, userId);
        }
        [Authorize(Roles = "ServiceStaff,Receptionist")]
        [HttpGet("check-attendance-personal")]
        public async Task<ResponseData<AttendancePersonal>> GetShiftQR(Guid id)
        {
            return await _attendante.GetPersonalShift(id);
        }
        [Authorize(Roles = "ServiceStaff,Receptionist")]
        [HttpPost("check-in-per")]
        public async Task<ResponseData<string>> CheckInPer(AttendancePersonal attendance)
        {
            return await _attendante.CheckInPersonal(attendance);
        }
        [Authorize(Roles = "ServiceStaff,Receptionist")]
        [HttpPost("check-out-per")]
        public async Task<ResponseData<string>> CheckOutPer(AttendancePersonal attendance)
        {
            return await _attendante.CheckOutPersonal(attendance);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllAttendance")]
        public async Task<ResponseData<List<AttendanceMonth>>> GetAllAttandanceMonth(int month = 0, int year = 0, int isDelete = -1)
        {
            return await _attendante.GetAllAttandanceMonth(month, year, isDelete);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("check-shift")]
        public async Task<ResponseData<string>> CheckShift(int shiftID, bool isCheckin)
        {
            return await _attendante.CheckLate(shiftID, isCheckin);
        }
        [Authorize(Roles = "ServiceStaff,Receptionist,Admin")]
        [HttpGet("get-attendance-month-by-user")]
        public async Task<ResponseData<ListPerAttenMonth>> GetAttandanceMonth(Guid idUser, int month = 0, int year = 0, int typeAttendance = Contant.AllAttendance)
        {
            return await _attendante.GetAttandanceMonth(idUser, month, year, typeAttendance);
        }



    }
}
