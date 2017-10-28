using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MySeenLib;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;
using static MySeenLib.MySeenWebApi;

namespace MySeenWeb.Controllers.Api
{
    public class SyncController : BaseApiController
    {
        private static SyncJsonData Map(Tracks model)
        {
            if (model == null) return new SyncJsonData();

            return new SyncJsonData
            {
                DataMode = (int)DataModes.Road,
                Name = model.Name,
                Type = model.Type,
                //Date = UmtTime.From(model.Date),
                Coordinates = model.Coordinates,
                Distance = model.Distance
            };
        }
        public IHttpActionResult Get(string userKey, int mode, int apiVersion)
        {
            try
            {
                if (apiVersion != ApiVersion)
                {
                    return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.NoLongerSupportedVersion });
                }

                var ac = new ApplicationDbContext();
                var userId = string.Empty;
                foreach (var user in ac.Users)
                {
                    if (Md5Tools.Get(user.Id) == userKey)
                    {
                        userId = user.Id;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(userId)) return Ok(new SyncJsonAnswer {Value = SyncJsonAnswer.Values.UserNotExist});

                if ((SyncModesApiData) mode == SyncModesApiData.GetRoads)
                {
                    var data = new List<SyncJsonData>();
                    data.AddRange(ac.Tracks.Where(f=> f.UserId == userId).Select(Map));

                    if (data.Any())
                    {
                        return Ok(data.AsEnumerable());
                    }
                    return Ok(new SyncJsonAnswer {Value = SyncJsonAnswer.Values.NoData});
                }
                return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
            }
            catch (Exception ex)
            {
                
            }
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.SomeErrorObtained });
        }
    }
}
