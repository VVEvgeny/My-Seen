using System.Web.Mvc;
using MySeenWeb.ActionFilters;
using MySeenWeb.Models;
using MySeenWeb.Models.ShareViewModels;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers
{
    //[RequireHttps]
    public class ShareController : BaseController
    {
        [BrowserActionFilter]
        public ActionResult Films(string id)
        {
            return View(new ShareViewModelFilmsMin(id));
        }

        [BrowserActionFilter]
        public ActionResult Serials(string id)
        {
            return View(new ShareViewModelSerialsMin(id));
        }

        [BrowserActionFilter]
        public ActionResult Books(string id)
        {
            return View(new ShareViewModelBooksMin(id));
        }

        [BrowserActionFilter]
        public ActionResult Events(string id)
        {
            return View(new ShareViewModelEventsMin(id));
        }

        [BrowserActionFilter]
        public ActionResult Tracks(string id)
        {
            return View(new ShareViewModelTracks(id, MarkersOnRoads, ReadUserSideStorage(UserSideStorageKeys.RoadsYear, 0)));
        }
    }
}