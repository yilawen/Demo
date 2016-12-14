using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
    public class UserModel : DbContext
    {
        public DbSet<User> Users {get; set;}

        public bool Authenticate(string username, string password)
        {
            var result = Users.Where(u => u.Username == username && u.Password == password).ToList();
            if (result.Count < 1) return false;
            else return true;
        }

        public List<User> GetAllUsers()
        {
            return Users.ToList();
        }

        public User GetUserByUsername(string username)
        {
            var result = Users.Where(u => u.Username == username).ToList();
            if (result.Count > 0) return result[0];
            else return null;
        }

        public bool AddUser(User user)
        {
            var result = GetUserByUsername(user.Username);
            if (result != null) return false;
            Users.Add(user);
            SaveChanges();
            return true;
        }

        public bool UpdateUser(User user)
        {
            User oldUser = Users.Where(u => u.Username == user.Username).First();
            if (oldUser == null) return false;
            oldUser.Email = "tset@tset";
            Entry(oldUser).State = EntityState.Modified;
            SaveChanges();
            return true;
        }

    }

    [Table("USER", Schema = "dbo")]
    public class User
    {
        public int UserId {get; set;}
        public string Username {get; set;}
        public string Password {get; set;}
        public string Email {get; set;}
    }
}