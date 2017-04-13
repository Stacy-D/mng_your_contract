using Microsoft.AspNet.Identity;
using System;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Класс ASP.NET Identity
    /// IRole інтерфейс 
    /// </summary>
    public class IdentityRole : IRole
    {
        /// <summary>
        /// Конструктор для ролі
        /// </summary>
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Конструктор (імя як аргумент)
        /// </summary>
        /// <param name="name"></param>
        public IdentityRole(string name) : this()
        {
            Name = name;
        }

        public IdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Роль ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Нейм ролі
        /// </summary>
        public string Name { get; set; }
    }
}
