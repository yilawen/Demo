using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using demo.Utilities;
using demo.Utilities.Entities;

namespace demo.Models
{
    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Menu> Menus { get; set; }

        public bool Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return false;
            password = Helper.MD5(password);
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
            user.Nickname = string.IsNullOrEmpty(user.Nickname) ? null : user.Nickname.Trim();
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

        public int GetUsersAmount()
        {
            return Users.Count();
        }

        public List<User> GetUsers(int offset, int amount)
        {
            int start = --offset * amount;
            return Users.OrderBy(u => u.Id).Skip(start).Take(amount).ToList();
        }

        public List<Menu> GetAllPermissions()
        {
            return this.Menus.ToList();
        }

        public List<Menu> GetPermissionsByUserId(string userId)
        {
            var data = this.Users.Where(u => u.Id == userId).Join(this.UserPermissions, u => u.Id, uP => uP.UserId, (u, uP) => uP).
                Join(Menus, uP => uP.MenuId, m => m.Id, (uP, m) => m);
            return data.ToList();
        }
    }
}