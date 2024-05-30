﻿using DATN.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Common
{
    public abstract class GennericRepository<T> : IGennericRepository<T> where T : class
    {
        protected DATNDbContext _context;

        public GennericRepository(DATNDbContext context)
        {
           _context = context;
        }

        public virtual T Add(T entity)
        {
            return _context
                .Add(entity)
                .Entity;
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            var x = predicate;
            return _context.Set<T>()
                .AsQueryable()
                .Where(predicate).ToList();
        }
        public virtual T Get(Guid id)
        {
            return _context.Find<T>(id);
        }

        public virtual IEnumerable<T> All()
        {
            return _context.Set<T>()
                .AsQueryable()
                .ToList();
        }

        public virtual T Update(T entity)
        {
            return _context.Update(entity)
                .Entity;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
