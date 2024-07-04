using DATN.Aplication.Common;
using DATN.Data.Entities;
using DATN.ViewModels.Common;

namespace DATN.Aplication.Repository.IRepository
{
    public interface IGuestRepository : IGenericRepository<Guest>
    {
        Task<bool> CheckEmailExist(string email);
        Task RemoveGuestByEmail(string email, Guid id);
        Task<bool> CheckUserExist(string user);
        Task<bool> CheckPhoneNumberExist(string phoneNumber);
        Task<bool> SoftDelete(DeleteRequest<Guid> request);
        Task<Guest> FindByEmail(string email);
    }
}