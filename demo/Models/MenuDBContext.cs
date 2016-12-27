using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using demo.Utilities;
using demo.Utilities.Entities;
using System.Data;

namespace demo.Models
{
    public class MenuDBContext : DbContext
    {
        public DbSet<Menu> Menus { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<User> Users { get; set; }

        public List<Menu> GetMenusByUserId(string userId)
        {
            var data = Users.Where(u => u.Id == userId).Join(UserPermissions, u => u.Id, uP => uP.UserId, (u, uP) => uP).
                Join(Menus, uP => uP.MenuId, m => m.Id, (uP, m) => m);
            return data.ToList();
        }

        public bool isMenuExists(Menu menu)
        {
            var result = this.Menus.Where(m => m.Code == menu.Code ||
                (m.ParentId == menu.ParentId && m.MenuName == menu.MenuName)).ToList();
            if (result.Count() > 0) return true;
            else return false;
        }

        public void AddOrUpdateMenu(Menu menu)
        {
            if (menu.Id == 0)
            {
                this.Menus.Add(menu);
                this.SaveChanges();
            }
            else
            {
                this.Menus.Attach(menu);
                this.Entry(menu).State = EntityState.Modified;
                this.SaveChanges();
            }
        }

        public void DeleteMenu(Menu menu)
        {
            this.Menus.Attach(menu);
            this.Menus.Remove(menu);
            this.SaveChanges();
        }

        public List<Menu> GetAllParentMenus()
        {
            return this.Menus.Where(m => m.ParentId == 0).ToList();
        }

        public List<Menu> GetAllMenus()
        {
            return this.Menus.ToList();
        }
    }
}