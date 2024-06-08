using DATN.ViewModels.Common.Location;

namespace DATN.ADMIN.IServices
{
    public interface IAddressService
    {
        Task<List<DataAdress>> GetProvinces();
        Task<List<DataAdress>> GetDistricts(string provinceCode);
        Task<List<DataAdress>> GetWards(string districtCode);
    }
}
