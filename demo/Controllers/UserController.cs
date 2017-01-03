﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wood.Models;
using Wood.Filters;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Web.Script.Serialization;
using Wood.Utilities;
using Wood.Models.Entities;

namespace Wood.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            if (Session["user"] != null) {
                return RedirectToAction("Notice");
            } else return View("Login");
        }

        public ActionResult Login(string username, string password)
        {
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
                        return Json(new { status = true });
                    }
                    else
                    {
                        return Json(new { status = false, message = "用户名或密码不正确" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, message = ex.Message });
                }
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
                ViewBag.menus = GetHomeMenus();
                ViewBag.users = userDB.GetAllUsers();
                return View("UserManage");
            }
        }

        [CheckUser]
        public JsonResult GetUsers()
        {
            int offset = Convert.ToInt32(Request.Form["page"]);
            int amount = Convert.ToInt32(Request.Form["rows"]);
            using (UserDBContext userDB = new UserDBContext())
            {
                Dictionary<string, object> data = new Dictionary<string,object>();
                data.Add("rows", userDB.GetUsers(offset, amount));
                data.Add("total", userDB.GetUsersAmount());
                return Json(data);
            }
        }

        [HttpPost]
        [CheckUser]
        public ActionResult AddUser(User user)
        {
            using (UserDBContext userDB = new UserDBContext())
            {
                try
                {
                    if (userDB.GetUserByUsername(user.Username) != null || userDB.GetUserByNickname(user.Nickname) != null)
                    {
                        return Json(new { status = false, message = "已存在的昵称或用户名" });
                    }
                    else
                    {
                        userDB.AddUser(user);
                        return Json(new { status = true });
                    }
                }
                catch(Exception ex)
                {
                    return Json(new { status = false, message = ex.Message });
                }
            }
        }

        [HttpPost]
        [CheckUser]
        public ActionResult UpdateUserInfo(string userStr, string[] properties)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            User user = serializer.Deserialize<User>(userStr);
            using (UserDBContext userDB = new UserDBContext())
            {
                try
                {
                    if (properties.Contains("Nickname") && userDB.GetUserByNickname(user.Nickname) != null)
                    {
                        return Json(new { status = false, message = "已存在该昵称" });
                    }
                    else
                    {
                        userDB.UpdateUser(user, properties);
                        return Json(new { status = true });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, message = ex.Message });
                }
            }
        }

        public ActionResult UpdateCurrentUserPassword(string password)
        {
            User user = (User)Session["user"];
            using (UserDBContext userDB = new UserDBContext())
            {
                try
                { 
                    userDB.UpdateUser(new User() { Id = user.Id, Password = password}, new string[]{"Password"});
                    return Json(new { status = true });
                }
                catch(Exception ex)
                {
                    return Json(new { status = false, message = ex.Message });
                }
            }
        }

        [HttpPost]
        [CheckUser]
        public ActionResult DeleteUser(string username)
        {
            using(UserDBContext userDB = new UserDBContext())
            {
                try
                {
                    userDB.DeleteUser(username);
                    return Json(new { status = true });
                }
                catch(Exception ex)
                {
                    return Json(new { status = false, message = ex.Message });
                }
            }
        }

        public ActionResult GetAllPermissions()
        {
            using (UserDBContext userDB = new UserDBContext())
            {
                return Json(Helper.MenusFormatNest(userDB.GetAllPermissions()));
            }
        }

        public ActionResult GetPermissions(string userId)
        {
            using (UserDBContext userDB = new UserDBContext())
            {
                return Json(userDB.GetPermissionsByUserId(userId));
            }
        }

        [CheckUser]
        public ActionResult Notice()
        { 
            using (MenuDBContext menuDB = new MenuDBContext())
            {
                User user = (User)Session["user"];
                List<Menu> menus = menuDB.GetMenusByUserId(user.Id);
                ViewBag.menus = GetHomeMenus();
                ViewBag.nickname = user.Nickname??user.Username;
                return View("Index");
            }
        }

        public ActionResult UpdateUserPemissions(string userId, int[] addedPmsIds, int[] deletedPmsIds)
        {
            using (UserDBContext userDB = new UserDBContext())
            {
                try
                {
                    userDB.UpdateUserPermissions(userId, addedPmsIds, deletedPmsIds);
                    return Json(new { status = true });
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, message = ex.Message });
                }
            }
        }
    }
}
