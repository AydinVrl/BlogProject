using Entity.Abstract;
using Microsoft.EntityFrameworkCore;
using Project.Repositories.Abstracts.Base;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Contracts
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : BaseClass
    {
        protected readonly AppDbContext _context;

        protected RepositoryBase(AppDbContext context)
        {
            _context = context;
        }


        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return trackChanges ? _context.Set<T>() : _context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindAllCondition(Expression<Func<T, bool>> condition, bool trackChanges)
        {
            return trackChanges
                ? _context.Set<T>().Where(condition)
                : _context.Set<T>().Where(condition).AsNoTracking();
        }

        public T? FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges)
        {
            return trackChanges
                 ? _context.Set<T>().Where(condition).SingleOrDefault()
                 : _context.Set<T>().Where(condition).AsNoTracking().SingleOrDefault();
        }

        public T? FindById(int id, bool trackChanges)
        {
            return trackChanges ? _context.Set<T>().SingleOrDefault(x => x.Id == id) : _context.Set<T>().AsNoTracking().SingleOrDefault(x => x.Id == id);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
