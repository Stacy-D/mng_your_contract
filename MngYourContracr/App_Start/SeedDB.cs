using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MngYourContracr.Models;

namespace MngYourContracr.MngYourContractDatabase
{
    public class SeedDB : DropCreateDatabaseAlways<CompanyContext> //DropCreateDatabaseIfModelChanges<CompanyContext>
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

            GetClients().ForEach(c => context.Clients.Add(c));
            GetEmployees().ForEach(c => context.Employees.Add(c));
            GetManagers().ForEach(c => context.Managers.Add(c));
            GetTeams().ForEach(c => context.Teams.Add(c));
            GetTasks().ForEach(c => context.Tasks.Add(c));
            GetProjects().ForEach(c => context.Projects.Add(c));
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

        //TODO write tasks to employees
        //TODO write projects and tasks end teams to manager ... etc

        private static List<Employee> GetEmployees()
        {
            employees = new List<Employee> {
     new Employee {
      EmployeeId = "1",
       User = new ApplicationUser {
        Id = "1",
         UserName = "Nastya",
         FirstName = "Nastya",
         LastName = "Danilochkona",
         PasswordHash = Password,
         SecurityStamp = "zvwGewj32",
         PhoneNumber = "+3800630034353",
         Email = "stacy_d@gmail.com",
         EmailConfirmed = true,
         Description = "Manager"
       }
     },
     new Employee {
      EmployeeId = "4",
       User = new ApplicationUser {
        Id = "4",
         UserName = "Olya",
         FirstName = "Olya",
         LastName = "Gavriluk",
         PasswordHash = Password,
         SecurityStamp = "zvwGewj32",
         PhoneNumber = "+380995425232",
         Email = "olga_g@gmail.com",
         EmailConfirmed = true,
         Description = "Employee"
       }
     }
    };

            anotherEmployees = new List<Employee> {
     new Employee {
      EmployeeId = "7",
       User = new ApplicationUser {
        Id = "7",
         UserName = "Ruslana",
         FirstName = "Ruslana",
         LastName = "Vynyavska",
         PasswordHash = Password,
         SecurityStamp = "zvwGewj32",
         PhoneNumber = "+380631234567",
         Email = "ruslana@coca-cola.com",
         EmailConfirmed = true,
         Description = "Our Client"
       }
     },
     new Employee {
      EmployeeId = "10",
       User = new ApplicationUser {
        Id = "10",
         UserName = "Maryana",
         FirstName = "Maryana",
         LastName = "Leskiv",
         PasswordHash = Password,
         SecurityStamp = "zvwGewj32",
         PhoneNumber = "+380677772377",
         Email = "leskiv96@gmail.com",
         EmailConfirmed = true,
         Description = "Employee"
       }
     }
    };

            var fullEmployeeList = new List<Employee>();
            employees.ForEach(e => fullEmployeeList.Add(e));
            anotherEmployees.ForEach(e => fullEmployeeList.Add(e));

            return fullEmployeeList.ToList();
        }

        private static List<Manager> GetManagers()
        {
            var managers = new List<Manager> {
     new Manager {
      ManagerId = "2",
       User = new ApplicationUser {
        Id = "2",
         UserName = "Ann",
         FirstName = "Ann",
         LastName = "Mitsan",
         PasswordHash = Password,
         SecurityStamp = "zvwGewj32",
         PhoneNumber = "+380663252323",
         Email = "tiperro@gmail.com",
         EmailConfirmed = true,
         Description = "Manager"
       }
     }
    };
            return managers;
        }

        private static List<Client> GetClients()
        {
            var clients = new List<Client> {
     new Client {
      ClientId = "3",
       User = new ApplicationUser {
        Id = "3",
         UserName = "Christina",
         FirstName = "Christina",
         LastName = "Zhibak",
         PasswordHash = Password,
         SecurityStamp = "zvwGewj32",
         PhoneNumber = "+380666666666",
         Email = "zhibak@apple.com",
         EmailConfirmed = true,
         Description = "Our Client",
       }
     }
    };

            return clients;
        }

        private static List<Team> GetTeams()
        {
            var teams = new List<Team> {
     new Team {
      TeamId = "1",
       ManagerId = "2",
       Employees = employees
     },
     new Team {
      TeamId = "2",
       ManagerId = "2",
       Employees = anotherEmployees
     }
    };
            return teams;
        }

        private static List<Task> GetTasks()
        {
            tasks = new List<Task> {
     new Task {
      TaskId = "1",
       Name = "Optimize DB for client list",
       Description = "Zarah v karmane!",
       Status = "OPENED",
       Deadline = new DateTime(2014, 05, 25),
       StartDate = DateTime.Now,
       EndDate = DateTime.MaxValue,
       EmployeeId = "1"
     },
     new Task {
      TaskId = "2",
       Name = "Dynamic list for viewers. Track online",
       Description = "Diplom v karmane!",
       Status = "CLOSED",
       Deadline = new DateTime(2014, 05, 25),
       StartDate = DateTime.Now,
       EndDate = DateTime.MaxValue,
       EmployeeId = "1"
     },
     new Task {
      TaskId = "3",
       Name = "Fill all lists",
       Description = "Proshe prostogo!",
       Status = "OPENED",
       Deadline = new DateTime(2014, 05, 25),
       StartDate = DateTime.Now,
       EndDate = DateTime.MaxValue,
       EmployeeId = "4"
     },
    };
            return tasks;
        }

        // TODO set project
        private static List<Project> GetProjects()
        {
            var projects = new List<Project> {
     new Project {
      ProjectId = "1",
       ClientId = "3",
       ManagerId = "2",
       TeamId = "1",
       Tasks = tasks,
       Name = "KMA zarah coming",
       Status = "OPENED",
       Description = "Po soto4ke kazhdomy",
       Deadline = new DateTime(2014, 05, 28),
       StartDate = DateTime.Now,
       EndDate = DateTime.MaxValue,
     }
    };
            return projects;
        }
    }
}