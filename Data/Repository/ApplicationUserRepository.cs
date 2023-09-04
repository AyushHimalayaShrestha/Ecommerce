using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;
using Model;

namespace Ecommerce9am.Data.Repository
{
    public class ApplicationUserRepository:Repository<ApplicationUser>, IApplicationUserRepository
    {
        public readonly ApplicationDBContext _db;

        public ApplicationUserRepository(ApplicationDBContext db):base(db)
        {
            _db = db;

        }

        public void Update(ApplicationUser  applicationUser)
        {
            _db.ApplicationUser.Update(applicationUser);
        }
    }
}
