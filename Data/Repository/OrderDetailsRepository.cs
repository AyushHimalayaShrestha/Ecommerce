using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;
using Model;

namespace Ecommerce9am.Data.Repository
{
    public class OrderDetailsRepository:Repository<OrderDetails>, IOrderDetailsRepository
    {
        public readonly ApplicationDBContext _db;

        public OrderDetailsRepository(ApplicationDBContext db):base(db)
        {
            _db = db;

        }

        public void Update(OrderDetails orderDetails)
        {
            _db.OrderDetails.Update(orderDetails);
        }
    }
}
