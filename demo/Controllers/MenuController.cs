using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using demo.Models;
using demo.Utilities.Entities;

namespace demo.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMenus(string userId)
        {
            using (MenuDBContext menuModel = new MenuDBContext())
            {
                List<Menu> menus = menuModel.GetMenusByUserId(userId);
                return Json(Utilities.Helper.MenusFormat(menus));
            }
        }

    }
}
