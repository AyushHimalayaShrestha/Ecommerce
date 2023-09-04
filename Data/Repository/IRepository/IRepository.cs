using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Numerics;

namespace Ecommerce9am.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? filter=null ,string? includeProperties=null);
        T FirstOrDefault( Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Create(T entity);
        void Delete(T entity);
        void DeleteRange(List<T> entities);

      
    }
}
