using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySeenWeb;
using MySeenWeb.Models;
using MySeenLib;

namespace MySeenWeb.Controllers
{
    public class ApiSyncController : ApiController
    {
        public static API_Data.FilmsRequestResponse Map(Films model)
        {
            if (model == null) return new API_Data.FilmsRequestResponse();

            return new API_Data.FilmsRequestResponse
            {
                IsFilm = true,
                Id = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rate = model.Rate
            };
        }
        public static API_Data.FilmsRequestResponse Map(Serials model)
        {
            if (model == null) return new API_Data.FilmsRequestResponse();

            return new API_Data.FilmsRequestResponse
            {
                IsFilm = false,
                Id = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rate = model.Rate,
                DateBegin=model.DateBegin,
                DateLast=model.DateLast,
                LastSeason=model.LastSeason,
                LastSeries=model.LastSeries,
            };
        }
        public IHttpActionResult Get(string user_key,int mode)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            if ((API_Data.ModesApiFilms)mode == API_Data.ModesApiFilms.All)
            {
                string user_id = string.Empty;
                if(ac.Users.Where(u=>u.UniqueKey == user_key).Count()==0)
                {
                    return Ok(new API_Data.RequestResponseAnswer { Value = API_Data.RequestResponseAnswer.Values.UserNotExist });
                }
                else
                {
                    user_id = ac.Users.Where(u => u.UniqueKey == user_key).Select(u => u.Id).First();
                }
                ac.Films.RemoveRange(ac.Films.Where(f => f.UserId == user_id && f.isDeleted == true));
                ac.Serials.RemoveRange(ac.Serials.Where(f => f.UserId == user_id && f.isDeleted == true));

                List<API_Data.FilmsRequestResponse> film = new List<API_Data.FilmsRequestResponse>();
                film.AddRange(ac.Films.Where(f => f.UserId == user_id).Select(Map));
                film.AddRange(ac.Serials.Where(f => f.UserId == user_id).Select(Map));

                foreach(Films f in ac.Films.Where(f => f.UserId == user_id && f.DateChange != null))
                {
                    f.DateChange = null;
                }
                foreach (Serials f in ac.Serials.Where(f => f.UserId == user_id && f.DateChange != null))
                {
                    f.DateChange = null;
                }

                if (film != null && film.Count()>0)
                {
                    return Ok(film.AsEnumerable());
                }
                return Ok(new API_Data.RequestResponseAnswer { Value = API_Data.RequestResponseAnswer.Values.NoData });
            }
            return Ok(new API_Data.RequestResponseAnswer { Value = API_Data.RequestResponseAnswer.Values.BadRequestMode });
        }
    }
}
