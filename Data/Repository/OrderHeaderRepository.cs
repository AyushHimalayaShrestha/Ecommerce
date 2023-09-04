using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;
using Model;

namespace Ecommerce9am.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        public readonly ApplicationDBContext _db;

        public OrderHeaderRepository(ApplicationDBContext db):base(db)
        {
            _db = db;

        }

        public void Update(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
           var orderFromDb=_db.OrderHeaders.FirstOrDefault(u=>u.Id == id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus=paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentID)
        {
            var orderFromDb= _db.OrderHeaders.FirstOrDefault(u=> u.Id == id);   
            if(!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId= sessionId;

            }
            if (!string.IsNullOrEmpty(paymentID))
            {
                orderFromDb.PaymentIntentId= paymentID;
                orderFromDb.PaymentDate=DateTime.Now;
            }

        }
    }
}
