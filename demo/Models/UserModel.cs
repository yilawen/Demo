using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Security;
using demo.Utility;

namespace demo.Models
{
    public class UserModel : DbContext
    {
        public DbSet<User> Users { get; set; }

        public bool Authenticate(string username, string password)
        {
            password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").ToLower();
            var result = Users.Where(u => u.Username == username && u.Password == password).Select(u => u.Username).FirstOrDefault();
            if (result == null) return false;
            else return true;
        }

        public User GetUserByUsername(string username)
        {
            var result = Users.Where(u => u.Username == username).FirstOrDefault();
            return result;
        }

        public User GetUserByUsernameOrNickname(string username, string nickname)
        {
            var result = Users.Where(u => u.Username == username || u.NickName == nickname).FirstOrDefault();
            return result;
        }

        public bool AddUser(User user)
        {
            var result = GetUserByUsernameOrNickname(user.Username, user.NickName);
            if (result != null) return false;
            user.Id = Guid.NewGuid().ToString();
            user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password.Trim(), "MD5").ToLower();
            user.LastLoginDate = new DateTime(1990, 1, 1);
            user.RoleGrade = (int)Role.General;
            Users.Add(user);
            SaveChanges();
            return true; 
        }

        public void UpdateUser(User user, string[] properties)
        {
            Users.Attach(user);
            var entry = Entry(user);
            foreach (var propertyName in properties)
            {
                entry.Property(propertyName).IsModified = true;
            }
            SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            return Users.ToList<User>();
        }
    }

    [Table("Users", Schema = "dbo")]
    public class User
    {
        public string Id { get; set; } //生成Guid字符串
        public string Username { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string LastLoginIP { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int RoleGrade { get; set; }
        public int Status { get; set; }
    }
}