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
            UserModel user = new UserModel();
            bool result = user.authenticate(username, password);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

    }
}
