using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;
using Model;

namespace Ecommerce9am.Data.Repository.IRepository
{
    public interface IOrderDetailsRepository:IRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetails);
    }
}
