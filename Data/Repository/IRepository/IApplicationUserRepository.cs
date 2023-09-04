using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;
using Model;

namespace Ecommerce9am.Data.Repository.IRepository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        void Update(ApplicationUser applicationUser);
    }
}
