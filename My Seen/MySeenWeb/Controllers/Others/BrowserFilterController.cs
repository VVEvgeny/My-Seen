using System.Web.Mvc;
using MySeenWeb.Controllers._Base;

namespace MySeenWeb.Controllers.Others
{
    public class BrowserFilterController : BaseController
    {
        // GET: BrowserFilter
        public ActionResult Index()
        {
            return View();
        }
    }
}