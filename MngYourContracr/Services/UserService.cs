using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MngYourContracr.Models;
using MngYourContracr.MngYourContractDatabase;

namespace MngYourContracr.Service
{
    public class UserService : Service<ApplicationUser>
    {
        public UserService(CompanyContext context)
            : base(context)
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new CompanyContext()));
        }

        internal UserManager<ApplicationUser> UserManager { get; private set; }

        public ApplicationUser FindUserById(string id)
        {
            return UserManager.FindById(id);
        }
    }
}