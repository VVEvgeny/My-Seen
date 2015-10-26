using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySeenWeb;
using MySeenWeb.Models;
using MySeenLib;
using System.Web.Http.Description;

namespace MySeenWeb.Controllers
{
    public class ApiSyncController : ApiController
    {
        public static MySeenWebApi.SyncJsonData Map(Films model)
        {
            if (model == null) return new MySeenWebApi.SyncJsonData();

            return new MySeenWebApi.SyncJsonData
            {
                IsFilm = true,
                Id = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rating = model.Rating,
                isDeleted = model.isDeleted
            };
        }
        public static Films MapToFilm(MySeenWebApi.SyncJsonData model, string user_id)
        {
            if (model == null) return new Films();

            return new Films
            {
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rating = model.Rating,
                isDeleted = model.isDeleted,
                UserId = user_id
            };
        }
        public static MySeenWebApi.SyncJsonData Map(Serials model)
        {
            if (model == null) return new MySeenWebApi.SyncJsonData();

            return new MySeenWebApi.SyncJsonData
            {
                IsFilm = false,
                Id = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                DateBegin=model.DateBegin,
                DateLast=model.DateLast,
                LastSeason=model.LastSeason,
                LastSeries=model.LastSeries,
                isDeleted = model.isDeleted
            };
        }
        public static Serials MapToSerial(MySeenWebApi.SyncJsonData model, string user_id)
        {
            if (model == null) return new Serials();

            return new Serials
            {
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                DateBegin = model.DateBegin,
                DateLast = model.DateLast,
                LastSeason = model.LastSeason,
                LastSeries = model.LastSeries,
                isDeleted = model.isDeleted,
                UserId = user_id
            };
        }
        public string GetUserId(string user_key)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            if (ac.Users.Where(u => u.UniqueKey == user_key).Count() != 0)
            {
                return ac.Users.Where(u => u.UniqueKey == user_key).Select(u => u.Id).First();
            }
            return string.Empty;
        }
        public IHttpActionResult Get(string user_key,int mode)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            string user_id = GetUserId(user_key);
            if (string.IsNullOrEmpty(user_id))
            {
                return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.UserNotExist });
            }
            if ((MySeenWebApi.SyncModesApiData)mode == MySeenWebApi.SyncModesApiData.GetAll)
            {
                List<MySeenWebApi.SyncJsonData> film = new List<MySeenWebApi.SyncJsonData>();
                film.AddRange(ac.Films.Where(f => f.UserId == user_id).Select(Map).Union(ac.Serials.Where(f => f.UserId == user_id).Select(Map)));
                //film.AddRange(ac.Films.Where(f => f.UserId == user_id).Select(Map));
                //film.AddRange(ac.Serials.Where(f => f.UserId == user_id).Select(Map));

                if (film != null && film.Count()>0)
                {
                    return Ok(film.AsEnumerable());
                }
                return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NoData });
            }
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string user_key, [FromUri]int mode, [FromBody] IEnumerable<MySeenWebApi.SyncJsonData> data)
        {
            if (data == null || (MySeenWebApi.SyncModesApiData)mode != MySeenWebApi.SyncModesApiData.PostAll)
            {
                return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
            }
            string user_id = GetUserId(user_key);
            if (string.IsNullOrEmpty(user_id))
            {
                return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.UserNotExist });
            }
            if (data.Count() > 0)
            {
                ApplicationDbContext ac = new ApplicationDbContext();
                foreach (MySeenWebApi.SyncJsonData film in data)
                {
                    if (film.IsFilm)
                    {
                        if (film.Id == null)
                        {
                            if (ac.Films.Where(f => f.Name == film.Name && f.UserId==user_id).Count() != 0)//с таким именем у нас уже есть
                            {
                                var filmBD = ac.Films.Where(f => f.Name == film.Name && f.UserId == user_id).First();
                                if(filmBD.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                                {
                                    filmBD.DateChange = film.DateChange;
                                    filmBD.DateSee = film.DateSee;
                                    filmBD.Genre = film.Genre;
                                    filmBD.isDeleted = film.isDeleted;
                                    filmBD.Rating = film.Rating;
                                }
                            }
                            else //нету нового с таким именем
                            {
                                ac.Films.Add(MapToFilm(film, user_id));
                            }
                        }
                        else //старый обновился
                        {
                            if (ac.Films.Where(f => f.Id == film.Id && f.UserId == user_id).Count() != 0)//с таким ID есть в БД, обновим
                            {
                                var filmBD = ac.Films.Where(f => f.Id == film.Id && f.UserId == user_id).First();
                                if (filmBD.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                                {
                                    filmBD.DateChange = film.DateChange;
                                    filmBD.DateSee = film.DateSee;
                                    filmBD.Genre = film.Genre;
                                    filmBD.isDeleted = film.isDeleted;
                                    filmBD.Rating = film.Rating;
                                    filmBD.Name = film.Name;
                                }
                            }
                            else
                            {
                                ac.Films.Add(MapToFilm(film, user_id));
                            }
                        }
                    }
                    else
                    {
                        if (film.Id == null)//Новый 
                        {
                            if (ac.Serials.Where(f => f.Name == film.Name && f.UserId == user_id).Count() != 0)//с таким именем у нас уже есть
                            {
                                var filmBD = ac.Serials.Where(f => f.Name == film.Name && f.UserId == user_id).First();
                                if (filmBD.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                                {
                                    filmBD.DateChange = film.DateChange;
                                    filmBD.Genre = film.Genre;
                                    filmBD.isDeleted = film.isDeleted;
                                    filmBD.Rating = film.Rating;
                                    filmBD.DateBegin = film.DateBegin;
                                    filmBD.DateLast = film.DateLast;
                                    filmBD.LastSeason = film.LastSeason;
                                    filmBD.LastSeries = film.LastSeries;
                                }
                            }
                            else //нету нового с таким именем
                            {
                                ac.Serials.Add(MapToSerial(film, user_id));
                            }
                        }
                        else //старый обновился
                        {
                            if (ac.Serials.Where(f => f.Id == film.Id && f.UserId == user_id).Count() != 0)//с таким ID есть в БД, обновим
                            {
                                var filmBD = ac.Serials.Where(f => f.Id == film.Id && f.UserId == user_id).First();
                                if (filmBD.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                                {
                                    filmBD.DateChange = film.DateChange;
                                    filmBD.Genre = film.Genre;
                                    filmBD.isDeleted = film.isDeleted;
                                    filmBD.Rating = film.Rating;
                                    filmBD.DateBegin = film.DateBegin;
                                    filmBD.DateLast = film.DateLast;
                                    filmBD.LastSeason = film.LastSeason;
                                    filmBD.LastSeries = film.LastSeries;
                                    filmBD.Name = film.Name;
                                }
                            }
                            else
                            {
                                ac.Serials.Add(MapToSerial(film, user_id));
                            }
                        }
                    }
                }
                ac.SaveChanges();
                return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NewDataRecieved });
            }
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
    }
}
