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

        public ActionResult Register()
        {
            List<Order> orders = new List<Order>();
            Item item1 = new Item("item1", 10);
            Item item2 = new Item("item2", 10);
            Item item3 = new Item("item3", 10);
            Item item4 = new Item("item4", 10);

            List<Item> items1 = new List<Item>(), items2 = new List<Item>();
            items1.Add(item1);
            items1.Add(item2);
            items2.Add(item3);
            items2.Add(item4);


            Order order1 = new Order(1, items1);
            Order order2 = new Order(2, items2);

            orders.Add(order1);
            orders.Add(order2);
            ViewBag.orders = orders;
            return View();
        }

    }
    public class Order
    { 
        public int orderId;
        public List<Item> items;


        public Order(int id, List<Item> items)
        {
            orderId = id;
            this.items = items;
        }

    }

    public class Item
    {
        public string itemName;
        public float price;
        public Item(string name, float price)
        {
            itemName = name;
            this.price = price;
        }
    }
}
