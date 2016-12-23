using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using demo.Models;

namespace demo.Utilities
{
    public class Helper
    {
        public static string MD5(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        }

        public static dynamic MenusFormat(List<Menu> menus)
        {
            Dictionary<int, dynamic> parentMenus = new Dictionary<int,dynamic>();
            for (int i = 0; i < menus.Count(); i++ )
            {
                if (menus[i].ParentId == 0)
                {
                    parentMenus.Add(menus[i].Id, new { MenuName = menus[i].MenuName, Children = new List<dynamic>() });
                    menus.Remove(menus[i]);
                }
            }

            foreach (var menu in menus)
            {
                if (parentMenus.ContainsKey(menu.ParentId))
                {
                    parentMenus[menu.ParentId].Children.Add(new { MenuName = menu.MenuName, LinkUrl = menu.LinkUrl });
                }
            }
            return parentMenus.Values;
        }
    }
}