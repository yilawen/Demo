using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using demo.Models;

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
            using (MenuModel menuModel = new MenuModel())
            {
                List<Menu> menus = menuModel.GetMenusByUserId(userId);
                return Json(Utilities.Helper.MenusFormat(menus));
            }
        }

    }
}
