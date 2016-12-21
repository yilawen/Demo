using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using demo.Models;
using demo.Filters;

namespace demo.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            if (Session["user"] != null) {
                ViewBag.user = Session["user"];
                return View("index");
            } else return View("Login");
        }

        public JsonResult Login(string username, string password)
        {
            using (UserModel userModel = new UserModel())
            {
                bool result = userModel.Authenticate(username, password);
                if (result) {
                    var user = userModel.GetUserByUsername(username);
                    user.LastLoginDate = System.DateTime.Now;
                    user.LastLoginIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    Session["user"] = user;
                    userModel.UpdateUser(user, new string[]{"LastLoginDate", "LastLoginIP"});
                }
                return Json(result, JsonRequestBehavior.DenyGet);
            }
        }

        [CheckUser]
        public RedirectToRouteResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [CheckUser]
        public ActionResult UserManage()
        {
            using (UserModel userModel = new UserModel())
            {
                ViewBag.users = userModel.GetAllUsers();
                return View("UserManage");
            }
        }

        [CheckUser]
        public JsonResult AddUser(User user)
        {
            using (UserModel userModel = new UserModel())
            {
                Dictionary<string, string> result = new Dictionary<string,string>();
                if (!userModel.AddUser(user))
                {
                    result.Add("status", "false");
                    result.Add("message", "已存在的昵称或用户名");
                } else {
                    result.Add("status", "true");
                }
                return Json(result, JsonRequestBehavior.DenyGet);
            }
        }
    }
}
