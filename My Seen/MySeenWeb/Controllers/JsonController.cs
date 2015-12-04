using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenWeb.Models.TablesLogic;

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
    }
}