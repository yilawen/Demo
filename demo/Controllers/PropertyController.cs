using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wood.Models;
using Wood.Models.Entities;
using Wood.Utilities;

namespace Wood.Controllers
{
    public class PropertyController : BaseController
    {
        //
        // GET: /Property/

        public ActionResult PropertyRepository()
        {
            ViewBag.menus = GetHomeMenus();
            return View("propertyRepository");
        }

        public ActionResult GetProperties()
        {
            int offset = Convert.ToInt32(Request.Form["page"]);
            int amount = Convert.ToInt32(Request.Form["rows"]);
            using (PropertyDBContext propertyDB = new PropertyDBContext())
            {
                return Json(new 
                { 
                    rows = propertyDB.GetProperties(offset, amount),
                    total = propertyDB.GetPropertiesAmount() 
                });
            }
        }

        public ActionResult UpdateProperty(ProductProperty property)
        {
            using (PropertyDBContext propertyDB = new PropertyDBContext())
            {
                try 
                {
                    propertyDB.UpdateProperty(property);
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
