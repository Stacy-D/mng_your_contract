using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MngYourContracr.Models;
using MngYourContracr.Service;

namespace MngYourContracr.MngYourContractDatabase
{
    public class SeedDB : DropCreateDatabaseIfModelChanges<CompanyContext> //DropCreateDatabaseIfModelChanges<CompanyContext>
    {
        public static UserManager<ApplicationUser> UserManager
        {
            get;
            private set;
        }

        public static string Password = "password";
        public static string Stamp = "";
        public static UserStore<ApplicationUser> Store = null;
        public static List<Employee> employees = null;
        public static List<Employee> anotherEmployees = null;
        public static List<Task> tasks = null;

        protected override void Seed(CompanyContext context)
        {
            base.Seed(context);
            createRolesandUsers(context);

        }
        private static void createRolesandUsers(CompanyContext context)
        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User 
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website				

                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@gmail.com";
                user.FirstName = "Admin";
                user.LastName = "admin";

                string userPWD = "adminadmin";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role 
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Employee role 
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Client"))
            {
                var role = new IdentityRole();
                role.Name = "Client";
                roleManager.Create(role);

            }
        }

    }
}