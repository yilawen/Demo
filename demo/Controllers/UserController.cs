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
using demo.Utilities;

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
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("status", false);
            result.Add("message", null);
            using (UserDBContext userDB = new UserDBContext())
            {
                try
                {
                    if (userDB.Authenticate(username, password))
                    {
                        User user = userDB.GetUserByUsername(username);
                        user.LastLoginDate = System.DateTime.Now;
                        user.LastLoginIP = Request.ServerVariables["REMOTE_ADDR"];
                        Session["user"] = user;
                        userDB.UpdateUser(user, new string[] { "LastLoginDate", "LastLoginIP" });
                        result["status"] = true;
                    }
                    else
                    {
                        result["message"] = "用户名或密码不正确";
                    }
                }
                catch (Exception ex)
                {
                    result["message"] = ex.Message;
                }
                return Json(result);
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
            using (UserDBContext userDB = new UserDBContext())
            {
                ViewBag.users = userDB.GetAllUsers();
                return View("UserManage");
            }
        }

        [CheckUser]
        public JsonResult GetUsers()
        {
            using (UserDBContext userDB = new UserDBContext())
            {
                return Json(userDB.GetAllUsers());
            }
        }

        [HttpPost]
        [CheckUser]
        public JsonResult AddUser(User user)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("status", false);
            result.Add("message", null);
            using (UserDBContext userDB = new UserDBContext())
            {
                try
                {
                    if (userDB.GetUserByUsername(user.Username) != null || userDB.GetUserByNickname(user.Nickname) != null)
                    {
                        result["message"] = "已存在的昵称或用户名";
                    }
                    else
                    {
                        userDB.AddUser(user);
                        result["status"] = true;
                    }
                }
                catch(Exception ex)
                {
                    result["message"] = ex.Message;
                }
                return Json(result);
            }
        }

        [HttpPost]
        [CheckUser]
        public JsonResult UpdateUserInfo()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            result.Add("status", false);
            result.Add("message", null);
            string userStr = Request.Form["user"];
            string propertiesStr = Request.Form["properties"];
            User user = serializer.Deserialize<User>(userStr);
            string[] properties = propertiesStr.Split(',');
            using (UserDBContext userDB = new UserDBContext())
            {
                try
                {
                    if (properties.Contains("Nickname") && userDB.GetUserByNickname(user.Nickname) != null)
                    {
                        result["message"] = "已存在的昵称";
                    }
                    else
                    {
                        userDB.UpdateUser(user, properties);
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

        [HttpPost]
        [CheckUser]
        public JsonResult DeleteUser(string username)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("status", false);
            result.Add("message", null);
            using(UserDBContext userDB = new UserDBContext())
            {
                try
                {
                    userDB.DeleteUser(username);
                    result["status"] = true;
                }
                catch(Exception ex)
                {
                    result["message"] = ex.Message;
                }
                return Json(result);
            }
        }

        [CheckUser]
        public ActionResult Notice()
        { 
            using (MenuDBContext menuDB = new MenuDBContext())
            {
                User user = (User)Session["user"];
                List<Menu> menus = menuDB.GetMenusByUserId(user.Id);
                ViewBag.menus = Helper.HomepageMenusFormat(menus);
                ViewBag.nickname = user.Nickname??user.Username;
                return View("Index");
            }
        }
    }
}
