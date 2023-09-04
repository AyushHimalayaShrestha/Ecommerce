using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;

namespace Ecommerce9am.Data.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void Update(Category product);
    }
}
