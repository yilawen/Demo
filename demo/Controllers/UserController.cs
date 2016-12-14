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
            return View("Login");
        }

        public JsonResult Login(string username, string password)
        {
            using (UserModel userModel = new UserModel())
            {
                bool result = userModel.Authenticate(username, password);
                if (result) Session["user"] = userModel.GetUserByUsername(username);
                return Json(result, JsonRequestBehavior.DenyGet);
            }
           
        }

        public void Logout()
        {
            Session.Abandon();
        }

        public ActionResult ShowRegister()
        {
            return View("Register");
        }

        public JsonResult Register(User user)
        {
            using (UserModel userModel = new UserModel())
            {
                if (user.Username == null || user.Password == null) return Json(false);
                return Json(userModel.AddUser(user), JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult UserDetail()
        {
            if (Session["user"] == null) return View("Login");
            ViewBag.user = Session["user"];
            return View();
        }
    }
}
