using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class ImageProductRepository : GenericRepository<ImageProduct>, IImageProductRepository
    {
        public ImageProductRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
