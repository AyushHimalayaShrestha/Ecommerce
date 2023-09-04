using Ecommerce9am.Data.Repository.IRepository;

namespace Ecommerce9am.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _db;
        public ICategoryRepository Category { get; set; }
        public IProductRepository product { get; set; }
        public IShoppingCartRepository shoppingCart { get; set; }
        public IOrderHeaderRepository orderHeader { get; set; }
        public IOrderDetailsRepository orderDetails { get; set; }
        public IApplicationUserRepository applicationUser { get; set; }


        public UnitOfWork(ApplicationDBContext db)
        {
            _db = db;
            Category=new CategoryRepository(db);
            product = new ProductRepository(db);
            shoppingCart=new ShoppingCartRepository(db);
            orderHeader = new OrderHeaderRepository(db);
            orderDetails = new OrderDetailsRepository(db);
            applicationUser = new ApplicationUserRepository(db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
