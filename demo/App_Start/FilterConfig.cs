using System.Web;
using System.Web.Mvc;
using demo.Filters;

namespace demo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckUserAttribute());
        }
    }
}