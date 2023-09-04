namespace Ecommerce9am.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get;  }
        public IProductRepository product { get; }  
        public IShoppingCartRepository shoppingCart { get; }
        public IOrderDetailsRepository orderDetails { get; }
        public IOrderHeaderRepository orderHeader { get; }
        public IApplicationUserRepository applicationUser { get; }
        void Save();
    }
}
