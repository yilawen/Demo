using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using demo.Models;

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

        public RedirectToRouteResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}
