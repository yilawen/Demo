using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using demo.Utilities;

namespace demo.Models
{
    public class MenuModel : DbContext
    {
        public DbSet<Menu> Menus { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<User> Users { get; set; }

        public List<Menu> GetMenusByUserId(string userId)
        {
            var data = Users.Where(u => u.Id == userId).Join(UserPermissions, u => u.Id, uP => uP.UserId, (u, uP) => new
            {
                MenuCode = uP.MenuCode,
            }).Join(Menus, uP => uP.MenuCode, m => m.Code, (uP, m) => m);
            return data.ToList();
        }
    }

    [Table("UserPermissions", Schema = "dbo")]
    public class UserPermission
    {
        public int Id { get; set; }
        public string MenuCode { get; set; }
        public string UserId { get; set; }
    }

    [Table("Menus", Schema = "dbo")]
    public class Menu
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string MenuName { get; set; }
        public string LinkUrl { get; set; }
        public bool Status { get; set; }
        public int ParentId { get; set; }
        public int? Sort { get; set; }
    }
}