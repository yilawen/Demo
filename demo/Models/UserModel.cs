using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using demo.Utility;

namespace demo.Models
{
    public class UserModel : DbContext
    {
        public DbSet<User> Users { get; set; }

        public bool Authenticate(string username, string password)
        {
            password = Helper.MD5(password.Trim());
            var result = Users.Where(u => u.Username == username && u.Password == password).Select(u => u.Username).FirstOrDefault();
            if (result == null) return false;
            else return true;
        }

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            var result = Users.Where(u => u.Username == username).FirstOrDefault();
            return result;
        }

        public User GetUserByNickname(string nickname)
        {
            if(string.IsNullOrEmpty(nickname)) return null;
            var result = Users.Where(u => u.Nickname == nickname).FirstOrDefault();
            return result;
        }

        public void AddUser(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            if (!string.IsNullOrEmpty(user.Nickname)) user.Nickname = user.Nickname.Trim();
            user.Username = user.Username.Trim();
            user.Password = Helper.MD5(user.Password.Trim());
            user.LastLoginDate = new DateTime(1990, 1, 1);
            user.RoleGrade = (int)Role.General;
            Users.Add(user);
            SaveChanges();
        }

        public void UpdateUser(User user, string[] properties)
        {
            if (!string.IsNullOrEmpty(user.Nickname)) user.Nickname = user.Nickname.Trim();
            Users.Attach(user);
            var entry = Entry(user);
            if (properties.Contains("Password")) {
                user.Password = Helper.MD5(user.Password.Trim());
            }
            foreach (var propertyName in properties)
            {
                entry.Property(propertyName.ToString()).IsModified = true;
            }
            SaveChanges();
        }

        public void DeleteUser(string username)
        {
            User user = Users.Where(u => u.Username == username).FirstOrDefault();
            Users.Remove(user);
            SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            return Users.ToList<User>();
        }

        public List<User> GetUsers(int amount, int start)
        {
            return Users.OrderBy(u => u.Id).Skip(start).Take(amount).ToList();
        }
    }

    [Table("Users", Schema = "dbo")]
    public class User
    {
        public string Id { get; set; } //生成Guid字符串
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string LastLoginIP { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int RoleGrade { get; set; }
        public int Status { get; set; }
    }
}