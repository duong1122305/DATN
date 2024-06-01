using DATN.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Common
{

    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DATNDbContext _context;

        public GenericRepository(DATNDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var entityEntry = await _context.Set<T>().AddAsync(entity); // Chỉ thêm entity vào context
            return entityEntry.Entity;                                  // Trả về entity đã được thêm
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            var entityEntry = _context.Set<T>().Update(entity); // Cập nhật entity trong context
            return await Task.FromResult(entityEntry.Entity);   // Trả về entity đã được cập nhật
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);       // Lấy entity theo Guid
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);       // Lấy entity theo int
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();       // Lấy tất cả entities
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync(); // Tìm entities theo biểu thức điều kiện
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();           // Lưu thay đổi vào cơ sở dữ liệu
        }
    }
}
