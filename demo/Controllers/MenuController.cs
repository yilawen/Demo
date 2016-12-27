using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using demo.Models;
using demo.Utilities.Entities;
using demo.Utilities;

namespace demo.Controllers
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

        public ActionResult AddOrUpdateMenu(Menu menu)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("status", false);
            result.Add("message", null);
            using (MenuDBContext menuDB = new MenuDBContext())
            {
                try
                {
                    if (menuDB.isMenuExists(menu))
                    {
                        result["message"] = "菜单已存在";
                    }
                    else
                    {
                        menuDB.AddOrUpdateMenu(menu);
                        result["status"] = true;
                    }
                }
                catch (Exception ex)
                {
                    result["message"] = ex.Message;
                }
                return Json(result);
            }
        }

        public ActionResult DeleteMenu(Menu menu)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("status", false);
            result.Add("message", null);
            using (MenuDBContext menuDB = new MenuDBContext())
            {
                try
                {
                    menuDB.DeleteMenu(menu);
                    result["status"] = true;
                }
                catch (Exception ex)
                {
                    result["message"] = ex.Message;
                }
                return Json(result);
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
                return Json(Helper.MenusFormat(menuDB.GetAllMenus()));
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
