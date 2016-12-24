using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using demo.Models;
using demo.Filters;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Web.Script.Serialization;

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
                return View("Index");
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
                    user.LastLoginIP = Request.ServerVariables["REMOTE_ADDR"];
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
        public JsonResult GetUsers()
        {
            using (UserModel userModel = new UserModel())
            {
                return Json(userModel.GetAllUsers());
            }
        }

        [HttpPost]
        [CheckUser]
        public JsonResult AddUser(User user)
        {
            using (UserModel userModel = new UserModel())
            {
                Dictionary<string, string> result = new Dictionary<string,string>();
                if (userModel.GetUserByUsername(user.Username) != null || userModel.GetUserByNickname(user.Nickname) != null) {
                    result.Add("status", "false");
                    result.Add("message", "已存在的昵称或用户名");
                } else {
                    userModel.AddUser(user);
                    result.Add("status", "true");
                }
                return Json(result, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        [CheckUser]
        public JsonResult UpdateUserInfo()
        {
            string userJson = Request.Form["user"];
            string propertiesStr = Request.Form["properties"];
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Deserialize<dynamic>(userJson);
            string[] properties = propertiesStr.Split(',');
            Dictionary<string, string> result = new Dictionary<string, string>();
            User user = new User() { Id = json["Id"], Password = json["Password"], Nickname = json["Nickname"], Status = Convert.ToInt32(json["Status"]) };
            using (UserModel userModel = new UserModel())
            {
                if (properties.Contains("Nickname") && userModel.GetUserByNickname(user.Nickname) != null) {
                    result.Add("status", "false");
                    result.Add("message", "已存在的昵称");
                } else {
                    userModel.UpdateUser(user, properties);
                    result.Add("status", "true");
                }
                return Json(result, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        [CheckUser]
        public JsonResult DeleteUser(string username)
        {
            using(UserModel userModel = new UserModel())
            {
                userModel.DeleteUser(username);
                return Json(true);
            }
        }

        [CheckUser]
        public ActionResult Notice()
        { 
            using (MenuModel menuModel = new MenuModel())
            {
                List<Menu> menus = menuModel.GetMenusByUserId(((User)Session["user"]).Id);
                ViewBag.menus = Utilities.Helper.MenusFormat(menus);
                return View("Index");
            }
        }
    }
}
