using System.Web;
using System.Web.Mvc;

namespace MagicVilla_Web_Rider
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}