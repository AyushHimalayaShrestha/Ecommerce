using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;

namespace Ecommerce9am.Data.Repository
{
    public class CategoryRepository:Repository<Category>, ICategoryRepository
    {
        public readonly ApplicationDBContext _db;

        public CategoryRepository(ApplicationDBContext db):base(db)
        {
            _db = db;

        }

        public void Update(Category product)
        {
            _db.Categories.Update(product);
        }
    }
}
