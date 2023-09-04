using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;
using Model;

namespace Ecommerce9am.Data.Repository.IRepository
{
    public interface IShoppingCartRepository:IRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCart);
    }
}
