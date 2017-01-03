using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wood.Models;
using Wood.Models.Entities;
using Wood.Utilities;

namespace Wood.Controllers
{
    public class BaseController : Controller
    {
        public List<Dictionary<string, object>> GetHomeMenus()
        {
            using (MenuDBContext menuDB = new MenuDBContext())
            {
                User user = (User)Session["user"];
                List<Menu> menus = menuDB.GetMenusByUserId(user.Id);
                return Helper.HomepageMenusFormat(menus);
            }
        }
    }
}
