using System.Collections.Generic;

namespace MngYourContracr.MySQLConnection
{

    public class UserTable
    {
        private MySQLDatabase _database;

        public UserTable(MySQLDatabase database)
        {
            _database = database;
        }

        public string GetUserName(string userId)
        {
            string commandText = "Select Name from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            return _database.GetStrValue(commandText, parameters);
        }

        public string GetUserId(string userName)
        {
            string commandText = "Select Id from Users where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            return _database.GetStrValue(commandText, parameters);
        }

        public IdentityUser GetUserById(string userId)
        {
            IdentityUser user = null;
            string commandText = "Select * from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            var rows = _database.Query(commandText, parameters);
            if (rows != null && rows.Count == 1)
            {
                var row = rows[0];
                user = new IdentityUser();
                user.Id = row["Id"];
                user.UserName = row["UserName"];
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"]; 
            }

            return user;
        }

        public List<IdentityUser> GetUserByName(string userName)
        {
            List<IdentityUser> users = new List<IdentityUser>();
            string commandText = "Select * from Users where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            var rows = _database.Query(commandText, parameters);
            foreach(var row in rows)
            {
                IdentityUser user = new IdentityUser();
                user.Id = row["Id"];
                user.UserName = row["UserName"];
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                users.Add(user);
            }

            return users;
        }

        public string GetPasswordHash(string userId)
        {
            string commandText = "Select PasswordHash from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", userId);

            var passHash = _database.GetStrValue(commandText, parameters);
            if(string.IsNullOrEmpty(passHash))
            {
                return null;
            }

            return passHash;
        }

        public int SetPasswordHash(string userId, string passwordHash)
        {
            string commandText = "Update Users set PasswordHash = @pwdHash where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@pwdHash", passwordHash);
            parameters.Add("@id", userId);

            return _database.Execute(commandText, parameters);
        }

        public string GetSecurityStamp(string userId)
        {
            string commandText = "Select SecurityStamp from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };
            var result = _database.GetStrValue(commandText, parameters);

            return result;
        }

        public int Insert(IdentityUser user)
        {
            string commandText = "Insert into Users (UserName, Id, PasswordHash, SecurityStamp) values (@name, @id, @pwdHash, @SecStamp)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", user.UserName);
            parameters.Add("@id", user.Id);
            parameters.Add("@pwdHash", user.PasswordHash);
            parameters.Add("@SecStamp", user.SecurityStamp);

            return _database.Execute(commandText, parameters);
        }

        private int Delete(string userId)
        {
            string commandText = "Delete from Users where Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            return _database.Execute(commandText, parameters);
        }

        public int Delete(IdentityUser user)
        {
            return Delete(user.Id);
        }

        public int Update(IdentityUser user)
        {
            string commandText = "Update Users set UserName = @userName, PasswordHash = @pswHash, SecurityStamp = @secStamp WHERE Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userName", user.UserName);
            parameters.Add("@pswHash", user.PasswordHash);
            parameters.Add("@secStamp", user.SecurityStamp);
            parameters.Add("@userId", user.Id);

            return _database.Execute(commandText, parameters);
        }
    }
}
