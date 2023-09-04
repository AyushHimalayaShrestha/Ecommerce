using Ecommerce9am.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce9am.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        public DbSet<T> dbset;

        public Repository(ApplicationDBContext db)
        {
            _db = db;
            dbset = db.Set<T>();
        }

        public void Create(T entity)
        {
           dbset.Add(entity);
        }

        public void Delete(T entity)
        {
          dbset.Remove(entity);
        }

        public void DeleteRange(List<T> entities)
        {
           dbset.RemoveRange(entities);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            query= query.Where(filter);
            if (includeProperties != null)
            {
                query = query.Include(includeProperties);
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query= dbset;
            if(filter!= null)
            {
                query = query.Where(filter);
            }
            if(includeProperties != null)
            {
                query = query.Include(includeProperties);
            }
            return query.ToList();
        }

      

     
    }
}
