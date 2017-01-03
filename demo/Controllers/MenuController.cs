using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wood.Models;
using Wood.Models.Entities;
using Wood.Utilities;
using System.Web.Script.Serialization;

namespace Wood.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/

        public ActionResult MenuManage()
        {
            ViewBag.menus = this.GetHomeMenus();
            return View("MenuManage");
        }

        public ActionResult AddOrUpdateMenu(string menuStr, string[] properties)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Menu menu = serializer.Deserialize<Menu>(menuStr);
            using (MenuDBContext menuDB = new MenuDBContext())
            {
                try
                {
                    if (menu.Id == 0)
                    {
                        if (menuDB.isMenuExists(menu))
                        {
                            return Json(new { status = false, message = "菜单已存在" });
                        }
                        menuDB.AddMenu(menu);
                        return Json(new { status = true });
                    }
                    else
                    {
                        if (properties.Contains("MenuName") && menuDB.isMenuNameExists(menu))
                        {
                            return Json(new { status = false, message = "菜单已存在" });
                        }
                        menuDB.UpdateMenu(menu, properties);
                        return Json(new { status = true });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, message = ex.Message });
                }
            }
        }

        public ActionResult DeleteMenu(Menu menu)
        {
            using (MenuDBContext menuDB = new MenuDBContext())
            {
                try
                {
                    menuDB.DeleteMenu(menu);
                    return Json(new { status = true });
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, ex.Message });
                }
            }
        }

        public ActionResult GetAllParentMenus()
        {
            using (MenuDBContext menuDB = new MenuDBContext())
            {
                return Json(menuDB.GetAllParentMenus());
            }
        }

        public ActionResult GetAllMenus()
        {
            using (MenuDBContext menuDB = new MenuDBContext())
            {
                return Json(Helper.MenusFormatNest(menuDB.GetAllMenus()), JsonRequestBehavior.AllowGet);
            }
        }

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
