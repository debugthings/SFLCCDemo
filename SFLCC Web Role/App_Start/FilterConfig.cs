using System.Web;
using System.Web.Mvc;

namespace SFLCC_Web_Role
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
