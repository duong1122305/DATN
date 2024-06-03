using DATN.Aplication.Common;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository.IRepository
{
    public interface IGuestRepository : IGenericRepository<Guest>
    {
        Task<bool> CheckEmailExist(string email);
        Task RemoveGuestByEmail(string email);
        Task<bool> CheckUserExist(string user);
        Task<bool> CheckPhoneNumberExist(string phoneNumber);
    }
}