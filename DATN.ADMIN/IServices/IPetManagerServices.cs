using DATN.Data.Entities;
using DATN.ViewModels.Common;

namespace DATN.ADMIN.IServices
{
    public interface IPetManagerServices
    {
        Task<ResponseData<List<PetType>>> GetAllTypes();
    }
}
