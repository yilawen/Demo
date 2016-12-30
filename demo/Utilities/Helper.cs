using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using demo.Models;
using demo.Models.Entities;

namespace demo.Utilities
{
    public class Helper
    {
        public static string MD5(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        }

        public static List<Dictionary<string, object>> HomepageMenusFormat(List<Menu> menus)
        {
            if (menus.Count() == 0) return null;
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            menus.ForEach(menu =>
            {
                if (menu.ParentId == 0)
                {
                    Dictionary<string, object> menuParent = new Dictionary<string, object>();
                    menuParent.Add("Id", menu.Id);
                    menuParent.Add("MenuName", menu.MenuName);
                    menuParent.Add("Children", new List<Dictionary<string, string>>());
                    result.Add(menuParent);
                }
            });
            menus.ForEach(menu =>
            {
                int index = result.FindIndex(mP => Convert.ToInt32(mP["Id"]) == menu.ParentId);
                if (index != -1)
                {
                    Dictionary<string, string> menuChild = new Dictionary<string, string>();
                    menuChild.Add("MenuName", menu.MenuName);
                    menuChild.Add("LinkUrl", menu.LinkUrl);
                    ((List<Dictionary<string, string>>)result[index]["Children"]).Add(menuChild);
                }
            });
            return result;
        }

        public static List<Dictionary<string, object>> MenusFormatNest(List<Menu> menus)
        {
            if (menus.Count() == 0) return null;
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            menus.ForEach(menu =>
            {
                if (menu.ParentId == 0)
                {
                    Dictionary<string, object> menuParent = new Dictionary<string, object>();
                    menuParent.Add("Id", menu.Id);
                    menuParent.Add("Code", menu.Code);
                    menuParent.Add("MenuName", menu.MenuName);
                    menuParent.Add("LinkUrl", menu.LinkUrl);
                    menuParent.Add("Sort", menu.Sort);
                    menuParent.Add("Status", menu.Status);
                    menuParent.Add("Children", new List<Menu>());
                    result.Add(menuParent);
                }
            });
            menus.ForEach(menu =>
            {
                int index = result.FindIndex(mP => Convert.ToInt32(mP["Id"]) == menu.ParentId);
                if (index != -1)
                {
                    ((List<Menu>)result[index]["Children"]).Add(menu);
                }
            });
            return result;
        }

        public static List<Dictionary<string, object>> MenusFormatList(List<Menu> menus)
        {
            if (menus.Count() == 0) return null;
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            menus.ForEach(menu =>
            {
                Dictionary<string, object> menuParent = new Dictionary<string, object>();
                menuParent.Add("id", menu.Id);
                menuParent.Add("code", menu.Code);
                menuParent.Add("menuName", menu.MenuName);
                menuParent.Add("linkUrl", menu.LinkUrl);
                menuParent.Add("sort", menu.Sort);
                menuParent.Add("status", menu.Status);
                if (menu.ParentId != 0)
                {
                    menuParent.Add("parentId", menu.ParentId);
                }
                result.Add(menuParent);
            });
            return result;
        }
    }
}