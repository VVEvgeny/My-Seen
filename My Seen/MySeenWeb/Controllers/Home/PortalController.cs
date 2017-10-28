using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenWeb.ActionFilters;
using MySeenWeb.Add_Code;
using MySeenWeb.Controllers._Base;
using MySeenWeb.Models.TablesLogic.Portal;


namespace MySeenWeb.Controllers.Home
{
    public class PortalController : BaseController
    {
        public PortalController(ICacheService cache) : base(cache)
        {
            
        }
        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult RateMem(string recordId, bool plus)
        {
            try
            {
                var logic = new MemesStatsLogic();
                return !logic.AddRate(recordId, User.Identity.GetUserId(), plus) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
            }
            return new JsonResult { Data = new { success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }
    }
}