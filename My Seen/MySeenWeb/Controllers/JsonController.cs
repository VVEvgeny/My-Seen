using System.Threading;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.Models;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers
{
    public class JsonController : BaseController
    {
        [Authorize]
        [HttpPost]
        public JsonResult GetShare(string name, string id)
        {
            switch (name.ToLower())
            {
                case "films":
                {
                    var logic = new FilmsLogic();
                    return Json(logic.GetShare(id, User.Identity.GetUserId()));
                }
                case "serials":
                {
                    var logic = new SerialsLogic();
                    return Json(logic.GetShare(id, User.Identity.GetUserId()));
                }
                case "books":
                {
                    var logic = new BooksLogic();
                    return Json(logic.GetShare(id, User.Identity.GetUserId()));
                }
                case "events":
                {
                    var logic = new EventsLogic();
                    return Json(logic.GetShare(id, User.Identity.GetUserId()));
                }
            }
            return Json("-");
        }

        [Authorize]
        [HttpPost]
        public JsonResult GenerateShare(string name, string id)
        {
            switch (name.ToLower())
            {
                case "films":
                {
                    var logic = new FilmsLogic();
                    return Json(logic.GenerateShare(id, User.Identity.GetUserId()));
                }
                case "serials":
                {
                    var logic = new SerialsLogic();
                    return Json(logic.GenerateShare(id, User.Identity.GetUserId()));
                }
                case "books":
                {
                    var logic = new BooksLogic();
                    return Json(logic.GenerateShare(id, User.Identity.GetUserId()));
                }
                case "events":
                {
                    var logic = new EventsLogic();
                    return Json(logic.GenerateShare(id, User.Identity.GetUserId()));
                }
            }
            return Json("-");
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteShare(string name, string id)
        {
            switch (name.ToLower())
            {
                case "films":
                {
                    var logic = new FilmsLogic();
                    return Json(logic.DeleteShare(id, User.Identity.GetUserId()));
                }
                case "serials":
                {
                    var logic = new SerialsLogic();
                    return Json(logic.DeleteShare(id, User.Identity.GetUserId()));
                }
                case "books":
                {
                    var logic = new BooksLogic();
                    return Json(logic.DeleteShare(id, User.Identity.GetUserId()));
                }
                case "events":
                {
                    var logic = new EventsLogic();
                    return Json(logic.DeleteShare(id, User.Identity.GetUserId()));
                }
            }
            return Json("-");
        }

        [Authorize]
        [HttpPost]
        public JsonResult GetPage(int? page,string search)
        {
            //Восстановить  Rpp вместо 3х записей
            if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                Defaults.CategoryBase.Indexes.Events)
            {
                //Thread.Sleep(5000);
                return Json(new HomeViewModelEvents(User.Identity.GetUserId(), page ?? 1, Rpp , search));
            }
            else if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                Defaults.CategoryBase.Indexes.Books)
            {
                //Thread.Sleep(5000);
                return Json(new HomeViewModelBooks(User.Identity.GetUserId(), page ?? 1, Rpp, search));
            }
            else if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                Defaults.CategoryBase.Indexes.Serials)
            {
                //Thread.Sleep(2000);
                return Json(new HomeViewModelSerials(User.Identity.GetUserId(), page ?? 1, Rpp, search));
            }
            else if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                Defaults.CategoryBase.Indexes.Films)
            {
                //Thread.Sleep(2000);
                return Json(new HomeViewModelFilms(User.Identity.GetUserId(), page ?? 1, Rpp, search));
            }
            return Json("NOT REALIZED");
        }
    }
}