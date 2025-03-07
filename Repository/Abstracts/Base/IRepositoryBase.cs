using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Abstracts.Base
{
    public interface IRepositoryBase<T> where T : BaseClass
    {
        IQueryable<T> FindAll(bool trackChanges);
        T? FindById(int id, bool trackChanges);

        IQueryable<T> FindAllCondition(Expression<Func<T, bool>> condition, bool trackChanges);

        T? FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
