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
        private UserModel userModel;
        public UserController()
        {
            userModel = new UserModel();
        }

        public ActionResult Index()
        {
            return View("Login");
        }

        public JsonResult Login(string username, string password)
        {
            bool result = userModel.Authenticate(username, password);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        public ActionResult ShowRegister()
        {
            return View("Register");
        }

        public JsonResult Register(User user)
        {
            if (user.Username == null || user.Password == null) return Json(false);
            return Json(userModel.AddUser(user), JsonRequestBehavior.DenyGet);
        }
    }
}
