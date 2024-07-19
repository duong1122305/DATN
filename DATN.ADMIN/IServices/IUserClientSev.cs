using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using System.Collections;

namespace DATN.ADMIN.IServices
{
    public interface IUserClientSev
    {
        Task<ResponseData<List<UserInfView>>> GetAll();

        Task<ResponseData<string>> GetById(string id);
        Task<ResponseData<string>> GetByIdRemove(string id);
        Task<ResponseData<string>> Login(UserLoginView user);
        Task<ResponseMail> ForgotPassword(string mail);
        Task<ResponseData<string>> UpdateUser(UserUpdateView userInfView, string id);
        Task<ResponseData<string>> activeUser(string id);
        Task<ResponseData<string>> Register(UserRegisterView userRegisterView);

        Task<ResponseData<string>> AddShuduleStaffMany(List<string> lstStaff, int idShift);
        Task<ResponseData<List<ScheduleView>>> GetAllCaNhanVien();

        //Task<UserInfView> statusUser(DeleteRequest<Guid> deleteRequest);
        Task<ResponseData<UserInfView>> GetInfoUser(string id);
        Task<ResponseData<UserChangePasswordView>> ChangePassword(UserChangePasswordView userChangePasswordView);
        Task<ResponseData<string>> CheckCodeOtp(string username, string code);
        Task<ResponseData<string>> ResetPass(UserResetPassView userResetPassView);
        Task<ResponseData<List<RoleView>>> ListPosition();
        Task<ResponseData<string>> AddRoleForUser(AddRoleForUserView addRoleForUserView);
        Task<ResponseData<string>> InsertOneDayScheduleForStaffSuddenly(List<string> listUser, int shift, DateTime dateTime);
    }
		Task<ResponseData<string>> UpdateImg(string url, string imgId, string id);
	}
}
