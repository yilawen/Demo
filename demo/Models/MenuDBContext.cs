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
    public class MenuDBContext : DbContext
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
}