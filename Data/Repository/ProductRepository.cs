using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;

namespace Ecommerce9am.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public readonly ApplicationDBContext _db;

        public ProductRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;

        }

        public void Update(Product product)
        {
            Product productToUpdate = _db.Products.FirstOrDefault(u=>u.ID == product.ID);
          if(product.ImageUrl != null)
            {
                productToUpdate.Title = product.Title;
                productToUpdate.Description = product.Description;
                productToUpdate.ImageUrl = product.ImageUrl;
                productToUpdate.Price = product.Price;
                productToUpdate.Stock = product.Stock;
                productToUpdate.Category = product.Category;
                productToUpdate.CategoryId = product.CategoryId;    

            }
            else
            {
                productToUpdate.Title = product.Title;
                productToUpdate.Description = product.Description;
                productToUpdate.Price = product.Price;
                productToUpdate.Stock = product.Stock;
                productToUpdate.Category = product.Category;
                productToUpdate.CategoryId = product.CategoryId;
                productToUpdate.ImageUrl = productToUpdate.ImageUrl;
            }
        }
    }
}
