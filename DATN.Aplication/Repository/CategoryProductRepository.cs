using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class CategoryProductRepository : GenericRepository<CategoryDetails>, ICategoryDetailRepository
    {
        public CategoryProductRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
