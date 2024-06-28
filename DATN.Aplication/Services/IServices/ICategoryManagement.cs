using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services.IServices
{
    public interface ICategoryManagement
    {
        public Task<ResponseData<string>> CreateCategory(CategoryView categoryView);
        public Task<ResponseData<string>> UpdateCategory(CategoryView categoryView);
        public Task<ResponseData<string>> RemoveCategory(int id);
        public Task<ResponseData<string>> Active(int id);
        public Task<ResponseData<List<CategoryView>>> ListCategory();
    }
}
