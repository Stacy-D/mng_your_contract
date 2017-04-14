using Microsoft.AspNet.Identity;
using System;

namespace MngYourContracr.MySQLConnection
{
    /// <summary>
    /// Класс ASP.NET Identity
    /// IUser інтерфейс  
    /// </summary>
    public class IdentityUser : IUser
    {
        /// <summary>
        /// Конструктор дефолт
        /// </summary>
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Конструктор (імя як аргумент)
        /// </summary>
        /// <param name="userName"></param>
        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        /// <summary>
        /// Юзер ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// юзернейм
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// пароль
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// безопасность 
        /// </summary>
        public string SecurityStamp { get; set; }
    }
}
