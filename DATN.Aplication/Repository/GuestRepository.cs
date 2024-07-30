using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace DATN.Aplication.Repository
{
    public class GuestRepository : GenericRepository<Guest>, IGuestRepository
    {
        public GuestRepository(DATNDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            if (string.IsNullOrEmpty(email.Trim()))
            {
                return false;
            }
            var result = await _context.Guests.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && x.IsComfirm == true);

            return result != null;
        }

        public Task<bool> CheckEmailExist(string email, Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckPhoneNumberExist(string phoneNumber)
        {
            var result = await _context.Guests.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
            return result != null;
        }

        public async Task<bool> CheckUserExist(string user)
        {
            var result = await _context.Guests.FirstOrDefaultAsync(x => x.UserName == user);

            return result != null;
        }

        public async Task<Guest> FindByEmail(string email)
        {
            return await _context.Guests.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && x.IsComfirm == false);
        }  
        public async Task<Guest> FindByEmailAll(string email)
        {
            return await _context.Guests.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() &&( !x.IsDeleted.HasValue||!x.IsDeleted.Value));
        }



        public async Task RemoveGuestByEmail(string email, Guid id)
        {
            var result = _context.Guests.Where(x => x.Email == email && x.IsComfirm == false && x.Id != id);
            if (result != null)
            {
                _context.Guests.RemoveRange(result);
                await _context.SaveChangesAsync();

            }

        }



        public async Task<bool> SoftDelete(DeleteRequest<Guid> request)
        {
            try
            {
                var result = await _context.Guests.FirstOrDefaultAsync(x => x.Id == request.ID);
                if (result != null)
                {
                    result.IsDeleted = request.IsDelete;
                    return await SaveChangesAsync() > 0;

                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
