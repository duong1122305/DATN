using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.Utilites;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DATN.Aplication.Services
{
    public class AttendanteMangarService : IAttendanteMangarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AttendanteMangarService(IUnitOfWork unitOfWork, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID)
        {
            try
            {

                var lstWorkShiftToday = await _unitOfWork.WorkShiftRepository.FindAsync(p => p.WorkDate.Date == DateTime.Today.Date);
                var lstSchedule = await _unitOfWork.EmployeeScheduleRepository.GetAllAsync();
                var lstAttendance = await _unitOfWork.EmployeeAttendanceRepository.GetAllAsync();
                var lstUser = _userManager.Users.ToList();
                var result = from user in lstUser
                             join st in lstSchedule on user.Id equals st.UserId
                             join ws in lstWorkShiftToday on st.WorkShiftId equals ws.Id
                             join at in lstAttendance on st.Id equals at.EmployeeScheduleId into attendanceData
                             from at in attendanceData.DefaultIfEmpty()
                             where ws.ShiftId == shiftID
                             select new AttendanceViewModel
                             {
                                 UserName = user.UserName,
                                 DateAttendace = DateTime.Today.ToString("dd/MM/yyyy"),
                                 CheckInTime = at != null && at.CheckInTime.HasValue ? at.CheckInTime.Value.ToString("hh:mm") : "0",
                                 CheckOutTime = at != null && at.CheckOutTime.HasValue ? at.CheckOutTime.Value.ToString("hh:mm") : "0",
                                 ID = at != null ? at.Id : 0,
                                 StaffName = user.FullName,
                                 ScheduleID = st.Id,
                             };
                var data = result.ToList();
                if (result == null || result.Count() == 0)
                {
                    return new ResponseData<List<AttendanceViewModel>>(false, "Không có lịch làm việc lúc này");
                }
                return new ResponseData<List<AttendanceViewModel>>(result.ToList());
            }
            catch (Exception)
            {

                return new ResponseData<List<AttendanceViewModel>>(false, "Đại vương ơi có bug rồi");
            }
        }
        public async Task<ResponseData<List<Shift>>> GetShiftNow()
        {
            try
            {
                var now = DateTime.Now.TimeOfDay;
                var lstShift = await _unitOfWork.ShiftRepository.GetAllAsync();
                var x = lstShift.ToList()[1].From.Add(TimeSpan.FromMinutes(-30)) < now && lstShift.ToList()[1].To.Add(TimeSpan.FromMinutes(30)) > now;
                lstShift = lstShift.Where(p => p.From.Add(TimeSpan.FromMinutes(-30)) < now && p.To.Add(TimeSpan.FromMinutes(30)) > now);
                if (lstShift == null || lstShift.Count() == 0)
                {
                    return new ResponseData<List<Shift>>(false, "Hiện tại không trong ca làm");
                }
                return new ResponseData<List<Shift>>(lstShift.ToList());
            }
            catch (Exception)
            {
                return new ResponseData<List<Shift>>(false, "Có lỗi khi lấy dữ liệu danh sách ca");
            }
        }
        public async Task<ResponseData<string>> CheckIn(int scheduleId, int attendanceID, bool isCheckin, Guid userId)
        {

            try
            {
                var nowTS = DateTime.Now.TimeOfDay;
                var now = DateTime.Now;
                var schedule = await _unitOfWork.EmployeeScheduleRepository.GetAsync(scheduleId);
                var workShift = await _unitOfWork.WorkShiftRepository.GetAsync(schedule.WorkShiftId);
                var shift = await _unitOfWork.ShiftRepository.GetAsync(workShift.ShiftId);
                if (shift.From.Add(TimeSpan.FromMinutes(-30)) <= nowTS && shift.From.Add(TimeSpan.FromMinutes(30)) >= nowTS)
                {


                    string attendanceBy = _userManager.FindByIdAsync(userId.ToString()).GetAwaiter().GetResult()!.UserName!;
                    var attendance = new EmployeeAttendance();
                    if (isCheckin)// kiểm tra diểm danh hay huỷ điểm danh
                    {
                        if (attendanceID == 0)// nếu điểm danh ms thì tạo ms
                        {

                            attendance.CheckInTime = DateTime.Now;
                            attendance.EmployeeScheduleId = scheduleId;
                            attendance.OtherNotes = $"({DateTime.Now.ToString("hh:mm")})Check in bởi: {attendanceBy}|";



                            await _unitOfWork.EmployeeAttendanceRepository.AddAsync(attendance);
                        }
                        else// điểm danh trên data có sẵn
                        {
                            attendance = await _unitOfWork.EmployeeAttendanceRepository.GetAsync(attendanceID);
                            if (attendance.CheckInTime != null)
                            {
                                return new ResponseData<string>(false, "Đã điểm danh trước đó ");
                            }
                            attendance.CheckInTime = DateTime.Now;
                            attendance.OtherNotes += $"({DateTime.Now.ToString("hh:mm")})Check in bởi: {attendanceBy}|";
                            await _unitOfWork.EmployeeAttendanceRepository.UpdateAsync(attendance);
                        }

                    }
                    else// nếu là huỷ điểm danh
                    {
                        attendance = await _unitOfWork.EmployeeAttendanceRepository.GetAsync(attendanceID);
                        var timeDitance = DateTime.Now - attendance.CheckInTime;
                        if (timeDitance > TimeSpan.FromMinutes(2))//////// kiểm tra thời gian điểm danh cách 2p thì ko thể huỷ
                        {
                            return new ResponseData<string>(false, "Chỉ có thể huỷ check in trong 2 phút ");
                        }
                        attendance.CheckInTime = null;
                        attendance.OtherNotes += $"({DateTime.Now.ToString("hh:mm")})Huỷ check in bởi: {attendanceBy}|";
                        await _unitOfWork.EmployeeAttendanceRepository.UpdateAsync(attendance);

                    }
                    var result = await _unitOfWork.SaveChangeAsync();
                    if (result > 0)
                    {
                        if (isCheckin)
                        {
                            return new ResponseData<string>("Check in thành công");
                        }
                        else
                        {
                            return new ResponseData<string>("Huỷ Check in thành công");
                        }
                    }

                    return new ResponseData<string>(false, "Thao tác không thành công");
                }
                return new ResponseData<string>(false, "Không trong thời gian điểm danh");
            }

            catch (Exception)
            {
                return new ResponseData<string>(false, "Có lỗi khi kết nối máy chủ");
            }
        }
        public async Task<ResponseData<string>> CheckOut(int scheduleId, int attendanceID, bool isCheckout, Guid userId)
        {
            try
            {
                var nowTS = DateTime.Now.TimeOfDay;
                var now = DateTime.Now;
                var schedule = await _unitOfWork.EmployeeScheduleRepository.GetAsync(scheduleId);
                var workShift = await _unitOfWork.WorkShiftRepository.GetAsync(schedule.WorkShiftId);
                var shift = await _unitOfWork.ShiftRepository.GetAsync(workShift.ShiftId);


                if (shift.To.Add(TimeSpan.FromMinutes(-30)) <= nowTS && shift.To.Add(TimeSpan.FromMinutes(30)) >= nowTS)
                {
                    string attendanceBy = _userManager.FindByIdAsync(userId.ToString()).GetAwaiter().GetResult()!.UserName;

                    var attendance = new EmployeeAttendance();
                    if (isCheckout)// kiểm tra check out hay huỷ check out
                    {
                        attendance = await _unitOfWork.EmployeeAttendanceRepository.GetAsync(attendanceID);
                        if (attendance == null || attendance.CheckInTime == null)
                        {
                            return new ResponseData<string>(false, "Bạn phải check in trước khi check out");
                        }
                        if (attendance.CheckOutTime != null)
                        {
                            return new ResponseData<string>(false, "Đã điểm danh trước đó ");
                        }
                        attendance.CheckOutTime = DateTime.Now;
                        attendance.OtherNotes += $"({DateTime.Now.ToString("hh:mm")})Check out bởi: {attendanceBy}|";
                        await _unitOfWork.EmployeeAttendanceRepository.UpdateAsync(attendance);


                    }
                    else// nếu là huỷ điểm danh
                    {
                        attendance = await _unitOfWork.EmployeeAttendanceRepository.GetAsync(attendanceID);
                        var timeDitance = DateTime.Now - attendance.CheckOutTime;
                        if (timeDitance > TimeSpan.FromMinutes(2))//////// kiểm tra thời gian checkout cách 2p thì ko thể huỷ
                        {
                            return new ResponseData<string>(false, "Chỉ có thể huỷ check-out trong 2 phút ");
                        }
                        attendance.CheckOutTime = null;
                        attendance.OtherNotes += $"({DateTime.Now.ToString("hh:mm")})Huỷ check out bởi: {attendanceBy}|";
                        await _unitOfWork.EmployeeAttendanceRepository.UpdateAsync(attendance);

                    }
                    var result = await _unitOfWork.SaveChangeAsync();
                    if (result > 0)
                    {
                        if (isCheckout)
                        {
                            return new ResponseData<string>("Check-out thành công");
                        }
                        else
                        {
                            return new ResponseData<string>("Huỷ check-out thành công");
                        }
                    }
                    return new ResponseData<string>(false, "Thao tác không thành công");
                }
                return new ResponseData<string>(false, "Không trong thời gian check out");
            }

            catch (Exception)
            {
                return new ResponseData<string>(false, "Có lỗi khi kết nối máy chủ");
            }
        }
        public async Task<ResponseData<AttendancePersonal>> GetPersonalShift(Guid id)
        {
            try
            {
                var now = DateTime.Now.TimeOfDay;
                var lstShift = await _unitOfWork.ShiftRepository.GetAllAsync();
                var lstWorkShiftToday = await _unitOfWork.WorkShiftRepository.FindAsync(p => p.WorkDate.Date == DateTime.Today.Date);
                var lstSchedule = await _unitOfWork.EmployeeScheduleRepository.GetAllAsync();
                var lstAttendance = await _unitOfWork.EmployeeAttendanceRepository.GetAllAsync();// đổ ẩn vào lstSchedule

                var lstCheckin = from s in lstShift// lấy danh sách ca làm thô của nhân viên
                                 join ws in lstWorkShiftToday on s.Id equals ws.ShiftId
                                 join es in lstSchedule on ws.Id equals es.WorkShiftId
                                 where es.UserId == id
                                 select new
                                 {
                                     ID = es.Id,
                                     Name = s.Name,
                                     Start = s.From,
                                     End = s.To,
                                     IsCheckined = ((es.EmployeeAttendances == null || !es.EmployeeAttendances.Any() || es.EmployeeAttendances.First().CheckInTime == null) ? false : true),
                                     IsCheckouted = ((es.EmployeeAttendances == null || !es.EmployeeAttendances.Any() || es.EmployeeAttendances.First().CheckOutTime == null) ? false : true),
                                     ShiftId = s.Id,
                                 };

                var lstShiftString = "Bạn không có ca hôm nay";
                int scheduleID = 0;
                string shiftNow = "Không trong ca làm";
                bool? typeAttendance = null;
                string attendanceShift = "";
                int shiftId = 0;
                if (lstCheckin.Count() > 0)
                {
                    lstShiftString = "";
                    foreach (var item in lstCheckin)
                    {
                        lstShiftString += item.Name.ToString() + ", ";
                    }

                    lstShiftString = lstShiftString.Substring(0, lstShiftString.Length - 2);

                    foreach (var item in lstCheckin.OrderBy(p => p.Start))
                    {
                        if ((item.Start.Add(TimeSpan.FromMinutes(-30)) <= now && item.Start.Add(TimeSpan.FromMinutes(30)) >= now && !item.IsCheckined) //kiểm tra xem có đủ ko
                            || (item.IsCheckined && !item.IsCheckouted && item.End.Add(TimeSpan.FromMinutes(-30)) <= now && item.End.Add(TimeSpan.FromMinutes(30)) >= now))
                        {
                            scheduleID = item.ID;
                            shiftId = item.ShiftId;
                            attendanceShift = item.Name.ToString();
                            shiftNow = item.Name;
                            if (!item.IsCheckined) typeAttendance = true;
                            else if (!item.IsCheckouted) typeAttendance = false;

                        }

                    }
                }

                return new ResponseData<AttendancePersonal>(new AttendancePersonal()
                {
                    ListShift = lstShiftString,
                    ScheduleID = scheduleID,
                    ShiftID = shiftId,
                    ShiftNow = shiftNow,
                    TypeAttendance = typeAttendance,
                    ShiftAttendance = attendanceShift
                });


            }
            catch (Exception)
            {
                return new ResponseData<AttendancePersonal>(false, "Bạn đang không có trong danh sách ca làm");
            }
        }
        public async Task<ResponseData<string>> CheckInPersonal(AttendancePersonal atendance)
        {
            try
            {
                var schedule = await _unitOfWork.EmployeeScheduleRepository.GetAsync(atendance.ScheduleID);
                if (schedule == null || schedule.UserId == Guid.Empty)
                {
                    return new ResponseData<string>(false, "Bạn không có lịch hôm nay");
                }

                var nowTS = DateTime.Now.TimeOfDay;
                var now = DateTime.Now;
                var workShift = await _unitOfWork.WorkShiftRepository.GetAsync(schedule.WorkShiftId);
                var shift = await _unitOfWork.ShiftRepository.GetAsync(workShift.ShiftId);
                if (shift.From.Add(TimeSpan.FromMinutes(-30)) <= nowTS && shift.From.Add(TimeSpan.FromMinutes(30)) >= nowTS)
                {
                    var employeAttendances = await _unitOfWork.EmployeeAttendanceRepository.FindAsync(p => p.EmployeeScheduleId == atendance.ScheduleID);
                    var employeAttendance = new EmployeeAttendance();
                    if (employeAttendances != null && employeAttendances.Count() > 0)// kiểm tra có lịch ko
                    {
                        employeAttendance = employeAttendances.First();
                        employeAttendance.CheckInTime = now;
                        employeAttendance.OtherNotes += $"({now.ToString("HH:mm")} )" + atendance.Note + "|";
                        await _unitOfWork.EmployeeAttendanceRepository.UpdateAsync(employeAttendance);
                    }
                    else
                    {
                        employeAttendance.CheckInTime = now;
                        employeAttendance.OtherNotes = $"({now.ToString("HH:mm")} )" + atendance.Note + "|";
                        employeAttendance.EmployeeScheduleId = atendance.ScheduleID;
                        await _unitOfWork.EmployeeAttendanceRepository.AddAsync(employeAttendance);
                    }
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseData<string>("Điểm danh thành công");
                }
                else
                {
                    return new ResponseData<string>(false, "Bạn không trong thời gian điểm danh");
                }

            }
            catch (Exception)
            {
                return new ResponseData<string>(false, "Có lỗi khi kết nối máy chủ");
            }
        }
        public async Task<ResponseData<string>> CheckOutPersonal(AttendancePersonal atendance)
        {
            try
            {
                var schedule = await _unitOfWork.EmployeeScheduleRepository.GetAsync(atendance.ScheduleID);
                if (schedule == null || schedule.UserId == Guid.Empty)
                {
                    return new ResponseData<string>(false, "Bạn không có lịch hôm nay");
                }

                var nowTS = DateTime.Now.TimeOfDay;
                var now = DateTime.Now;
                var workShift = await _unitOfWork.WorkShiftRepository.GetAsync(schedule.WorkShiftId);
                var shift = await _unitOfWork.ShiftRepository.GetAsync(workShift.ShiftId);
                if (shift.To.Add(TimeSpan.FromMinutes(-30)) <= nowTS && shift.To.Add(TimeSpan.FromMinutes(30)) >= nowTS)
                {
                    var employeAttendances = await _unitOfWork.EmployeeAttendanceRepository.FindAsync(p => p.EmployeeScheduleId == atendance.ScheduleID);
                    var employeAttendance = new EmployeeAttendance();
                    if (employeAttendances != null && employeAttendances.Count() > 0 && employeAttendances.First().CheckInTime.HasValue)// kiểm tra có lịch ko
                    {
                        employeAttendance = employeAttendances.First();
                        employeAttendance.CheckOutTime = now;
                        employeAttendance.OtherNotes += $"({now.ToString("HH:mm")} )" + atendance.Note + "|";
                        await _unitOfWork.EmployeeAttendanceRepository.UpdateAsync(employeAttendance);
                    }
                    else
                    {
                        return new ResponseData<string>(false, "Phải check-in trước khi check-out");
                    }
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseData<string>("Điểm danh thành công");
                }
                else
                {
                    return new ResponseData<string>(false, "Bạn không trong thời gian điểm danh");
                }

            }
            catch (Exception)
            {
                return new ResponseData<string>(false, "Có lỗi khi kết nối máy chủ");
            }
        }

        public async Task<ResponseData<List<AttendanceMonth>>> GetAllAttandanceMonth(int month = 0, int year = 0, int isDelete = -1)
        {
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }

            if (year == 0)
            {
                year = DateTime.Now.Year;
            }

            try
            {
                var lstShift = await _unitOfWork.ShiftRepository.GetAllAsync();
                var lstWorkShiftToday = await _unitOfWork.WorkShiftRepository.FindAsync(p => p.WorkDate.Month == month && p.WorkDate.Year == year);
                var lstSchedule = await _unitOfWork.EmployeeScheduleRepository.GetAllAsync();
                var lstAttendance = await _unitOfWork.EmployeeAttendanceRepository.GetAllAsync();
                var lstUser = _userManager.Users.ToList();
                var now = DateTime.Now;

                var result = from user in lstUser
                             join st in lstSchedule on user.Id equals st.UserId
                             join ws in lstWorkShiftToday on st.WorkShiftId equals ws.Id
                             join s in lstShift on ws.ShiftId equals s.Id
                             join at in lstAttendance on st.Id equals at.EmployeeScheduleId into attendanceData
                             from at in attendanceData.DefaultIfEmpty()
                             where isDelete == -1 || (user.IsDeleted == false && isDelete == 0) || (user.IsDeleted == false && isDelete == -1)
                             group new { st, at, ws, s } by new { user.Id, user.FullName, user.UserName } into grouped
                             select new AttendanceMonth
                             {
                                 IdUser = grouped.Key.Id,
                                 FullName = grouped.Key.FullName,
                                 UserName = grouped.Key.UserName,
                                 ScheduleWorkingDays = grouped.Count(g => g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year),
                                 ActualWorkingDays = grouped.Count(g => g.at != null && g.at.CheckOutTime.HasValue && g.at.CheckInTime.HasValue && g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year),

                                 TotalLeaveDays = grouped.Count(g => (g.at == null || !g.at.CheckInTime.HasValue || !g.at.CheckOutTime.HasValue) && g.ws.WorkDate < now && g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year),

                                 TotalWorkingHours = grouped.Where(g => g.at != null && g.at.CheckOutTime.HasValue && g.at.CheckInTime.HasValue && g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year)
                                .Sum(g => (g.at.CheckOutTime!.Value - g.at.CheckInTime!.Value).TotalHours > 5.0 ? 5.0
                                : (g.at.CheckOutTime.Value - g.at.CheckInTime.Value).TotalHours)// 1 ca tính 5h thôi
,

                                 DaysLate = grouped.Count(g => g.at != null && g.at.CheckOutTime.HasValue && g.at.CheckInTime.HasValue && g.at.CheckInTime.Value.TimeOfDay > g.s.From
                                 && g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year),

                                 DaysLeftEarly = grouped.Count(g => g.at != null && g.at.CheckOutTime.HasValue && g.at.CheckInTime.HasValue && g.at.CheckOutTime.Value.TimeOfDay < g.s.To
                                 && g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year)
                             };

                if (result == null || result.Count() == 0)
                {
                    return new ResponseData<List<AttendanceMonth>>(false, "Tháng này không có người làm");
                }
                return new ResponseData<List<AttendanceMonth>>(result.ToList());
            }
            catch (Exception)
            {
                return new ResponseData<List<AttendanceMonth>>(false, "Tháng này không có người làm");
            }
        }
        public async Task<ResponseData<ListPerAttenMonth>> GetAttandanceMonth(Guid idUser, int month = 0, int year = 0, int typeAttendance = Contant.AllAttendance)
        {


            if (month == 0)
            {
                month = DateTime.Now.Month;
            }

            if (year == 0)
            {
                year = DateTime.Now.Year;
            }

            try
            {
                var lstShift = await _unitOfWork.ShiftRepository.GetAllAsync();
                var lstWorkShiftToday = await _unitOfWork.WorkShiftRepository.FindAsync(p => p.WorkDate.Month == month && p.WorkDate.Year == year);
                var lstSchedule = await _unitOfWork.EmployeeScheduleRepository.GetAllAsync();
                var lstAttendance = await _unitOfWork.EmployeeAttendanceRepository.GetAllAsync();
                var lstUser = _userManager.Users.Where(p => p.Id == idUser).ToList();
                var now = DateTime.Now;

                var dataMain = (from user in lstUser
                                join st in lstSchedule on user.Id equals st.UserId
                                join ws in lstWorkShiftToday on st.WorkShiftId equals ws.Id
                                join s in lstShift on ws.ShiftId equals s.Id
                                join at in lstAttendance on st.Id equals at.EmployeeScheduleId into attendanceData
                                from at in attendanceData.DefaultIfEmpty()
                                group new { st, at, ws, s } by new { user.Id, user.FullName, user.UserName } into grouped
                                select new AttendanceMonth
                                {
                                    IdUser = grouped.Key.Id,
                                    FullName = grouped.Key.FullName,
                                    UserName = grouped.Key.UserName,
                                    ScheduleWorkingDays = grouped.Count(g => g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year),
                                    ActualWorkingDays = grouped.Count(g => g.at != null && g.at.CheckOutTime.HasValue && g.at.CheckInTime.HasValue && g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year),

                                    TotalLeaveDays = grouped.Count(g => (g.at == null || !g.at.CheckInTime.HasValue || !g.at.CheckOutTime.HasValue) && g.ws.WorkDate < now && g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year),

                                    TotalWorkingHours = grouped.Where(g => g.at != null && g.at.CheckOutTime.HasValue && g.at.CheckInTime.HasValue && g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year)
                                   .Sum(g => (g.at.CheckOutTime!.Value - g.at.CheckInTime!.Value).TotalHours > 5.0 ? 5.0
                                   : (g.at.CheckOutTime.Value - g.at.CheckInTime.Value).TotalHours)// 1 ca tính 5h thôi
   ,

                                    DaysLate = grouped.Count(g => g.at != null && g.at.CheckOutTime.HasValue && g.at.CheckInTime.HasValue && g.at.CheckInTime.Value.TimeOfDay > g.s.From
                                    && g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year),

                                    DaysLeftEarly = grouped.Count(g => g.at != null && g.at.CheckOutTime.HasValue && g.at.CheckInTime.HasValue && g.at.CheckOutTime.Value.TimeOfDay < g.s.To
                                    && g.ws.WorkDate.Month == month && g.ws.WorkDate.Year == year)
                                }).FirstOrDefault();

                var result = from user in lstUser
                             join st in lstSchedule on user.Id equals st.UserId
                             join ws in lstWorkShiftToday on st.WorkShiftId equals ws.Id
                             join s in lstShift on ws.ShiftId equals s.Id
                             join at in lstAttendance on st.Id equals at.EmployeeScheduleId into attendanceData
                             from at in attendanceData.DefaultIfEmpty()
                             where typeAttendance == -1
                             || (typeAttendance == 1 && at != null && at.CheckInTime.HasValue && at.CheckOutTime.HasValue)
                             || (typeAttendance == 0 && (at == null || !at.CheckInTime.HasValue || !at.CheckOutTime.HasValue))
                             orderby ws.WorkDate
                             select new AttendancePerMonth
                             {
                                 Date = ws.WorkDate.Date.ToString("dd/MM/yyy"),
                                 Shift = s.Name,
                                 CheckinAt = at != null && at.CheckInTime.HasValue ? at.CheckInTime.Value.ToString("HH: mm") : "Không check in",
                                 CheckoutAt = at != null && at.CheckOutTime.HasValue ? at.CheckOutTime.Value.ToString("HH: mm") : "Không check out",
                                 TimeOfShift = s.From.ToString(@"hh\:mm") + " - " + s.To.ToString(@"hh\:mm"),
                                 History = at != null && at.OtherNotes != null ? at.OtherNotes : "Chưa có lịch sử"
                             };

                if (result == null || result.Count() == 0 || dataMain == null || string.IsNullOrEmpty(dataMain.FullName))
                {
                    return new ResponseData<ListPerAttenMonth>(false, "Không có data nhân viên này ở tháng này");
                }
                return new ResponseData<ListPerAttenMonth>(new ListPerAttenMonth()
                {
                    AllData = dataMain,
                    AttendancePerMonths = result.ToList()
                });

            }
            catch (Exception ex)
            {
                return new ResponseData<ListPerAttenMonth>(false, "Tháng này không có người làm" + ex.Message);
            }
        }

        public async Task<ResponseData<string>> CheckLate(int shiftID, bool isCheckin = true)
        {
            try
            {
                var shift = await _unitOfWork.ShiftRepository.GetAsync(shiftID);
                var now = DateTime.Now.TimeOfDay;
                if (isCheckin)
                {
                    if (shift.From < now)
                    {
                        return new ResponseData<string>("muộn " + Math.Ceiling((now - shift.From).TotalMinutes) + " phút");
                    }
                    return new ResponseData<string>("");

                }
                else
                {
                    if (shift.To > now)
                    {
                        return new ResponseData<string>("sớm " + Math.Ceiling((shift.To - now).TotalMinutes) + " phút");
                    }
                    return new ResponseData<string>("");
                }
            }
            catch (Exception)
            {

                return new ResponseData<string>(false, "Data truyền vào sai");
            }
        }
    }
}
