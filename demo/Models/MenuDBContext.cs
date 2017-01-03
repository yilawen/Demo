﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using Wood.Utilities;
using Wood.Models.Entities;
using System.Data;

namespace Wood.Models
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

        public bool isMenuNameExists(Menu menu)
        {
            var result = Menus.Where(m => m.ParentId == menu.ParentId && m.MenuName == menu.MenuName).FirstOrDefault();
            if (result != null) return true;
            else return false;
        }

        public void AddMenu(Menu menu)
        {
            Menus.Add(menu);
            SaveChanges();
        }

        public void UpdateMenu(Menu menu, string[] properties)
        {
            Menus.Attach(menu);
            var entry = Entry(menu);
            foreach (var propertyName in properties)
            {
                entry.Property(propertyName.ToString()).IsModified = true;
            }
            SaveChanges();
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