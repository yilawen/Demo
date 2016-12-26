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
using demo.Utilities.Entities;

namespace demo.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            if (Session["user"] != null) {
                return RedirectToAction("Notice");
            } else return View("Login");
        }

        public JsonResult Login(string username, string password)
        {
            using (UserDBContext userModel = new UserDBContext())
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
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        [CheckUser]
        public ActionResult UserManage()
        {
            using (UserDBContext userModel = new UserDBContext())
            {
                ViewBag.users = userModel.GetAllUsers();
                return View("UserManage");
            }
        }

        [CheckUser]
        public JsonResult GetUsers()
        {
            using (UserDBContext userModel = new UserDBContext())
            {
                return Json(userModel.GetAllUsers());
            }
        }

        [HttpPost]
        [CheckUser]
        public JsonResult AddUser(User user)
        {
            using (UserDBContext userModel = new UserDBContext())
            {
                Dictionary<string, object> result = new Dictionary<string,object>();
                if (userModel.GetUserByUsername(user.Username) != null || userModel.GetUserByNickname(user.Nickname) != null) {
                    result.Add("status", false);
                    result.Add("message", "已存在的昵称或用户名");
                } else {
                    userModel.AddUser(user);
                    result.Add("status", true);
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
            var json = serializer.Deserialize<Dictionary<string, string>>(userJson);
            string[] properties = propertiesStr.Split(',');
            Dictionary<string, object> result = new Dictionary<string, object>();
            User user = new User() { Id = json["Id"], Password = json["Password"], Nickname = json["Nickname"], Status = Convert.ToInt32(json["Status"]) };
            using (UserDBContext userModel = new UserDBContext())
            {
                if (properties.Contains("Nickname") && userModel.GetUserByNickname(user.Nickname) != null) {
                    result.Add("status", false);
                    result.Add("message", "已存在的昵称");
                } else {
                    userModel.UpdateUser(user, properties);
                    result.Add("status", true);
                }
                return Json(result, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        [CheckUser]
        public JsonResult DeleteUser(string username)
        {
            using(UserDBContext userModel = new UserDBContext())
            {
                userModel.DeleteUser(username);
                return Json(true);
            }
        }

        [CheckUser]
        public ActionResult Notice()
        { 
            using (MenuDBContext menuModel = new MenuDBContext())
            {
                User user = (User)Session["user"];
                List<Menu> menus = menuModel.GetMenusByUserId(user.Id);
                ViewBag.menus = Utilities.Helper.HomepageMenusFormat(menus);
                ViewBag.username = user.Username;
                return View("Index");
            }
        }
    }
}
