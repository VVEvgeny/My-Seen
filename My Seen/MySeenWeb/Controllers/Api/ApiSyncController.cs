using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using MySeenWeb.Models;
using MySeenLib;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers
{
    public class ApiSyncController : ApiController
    {
        public static MySeenWebApi.SyncJsonData Map(Films model)
        {
            if (model == null) return new MySeenWebApi.SyncJsonData();

            return new MySeenWebApi.SyncJsonData
            {
                DataMode = (int)MySeenWebApi.DataModes.Film,
                Id = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rating = model.Rating
            };
        }
        public static Films MapToFilm(MySeenWebApi.SyncJsonData model, string userId)
        {
            if (model == null) return new Films();

            return new Films
            {
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rating = model.Rating,
                UserId = userId
            };
        }
        public static MySeenWebApi.SyncJsonData Map(Serials model)
        {
            if (model == null) return new MySeenWebApi.SyncJsonData();

            return new MySeenWebApi.SyncJsonData
            {
                DataMode = (int)MySeenWebApi.DataModes.Serial,
                Id = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                DateBegin = model.DateBegin,
                DateLast = model.DateLast,
                LastSeason = model.LastSeason,
                LastSeries = model.LastSeries
            };
        }
        public static Serials MapToSerial(MySeenWebApi.SyncJsonData model, string userId)
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
                UserId = userId
            };
        }
        public static MySeenWebApi.SyncJsonData Map(Books model)
        {
            if (model == null) return new MySeenWebApi.SyncJsonData();

            return new MySeenWebApi.SyncJsonData
            {
                DataMode = (int)MySeenWebApi.DataModes.Book,
                Id = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                Authors = model.Authors,
                DateRead = model.DateRead
            };
        }
        public static Books MapToBook(MySeenWebApi.SyncJsonData model, string userId)
        {
            if (model == null) return new Books();

            return new Books
            {
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                UserId = userId,
                Authors = model.Authors,
                DateRead = model.DateRead
            };
        }
        public string GetUserId(string userKey)
        {
            var ac = new ApplicationDbContext();
            if (ac.Users.Any(u => u.UniqueKey == userKey))
            {
                return ac.Users.First(u => u.UniqueKey == userKey).Id;
            }
            return string.Empty;
        }
        public IHttpActionResult Get()
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
        public IHttpActionResult Get(string userKey)
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
        public IHttpActionResult Get(string userKey, int mode)
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NoLongerSupportedVersion });
        }
        public IHttpActionResult Get(string userKey, int mode, int apiVersion)
        {
            var logger = new NLogLogger();
            const string methodName = "public IHttpActionResult Get(string userKey, int mode, int apiVersion)";
            try
            {
                if (apiVersion != MySeenWebApi.ApiVersion)
                {
                    return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NoLongerSupportedVersion });
                }

                var ac = new ApplicationDbContext();
                var userId = GetUserId(userKey);
                if (string.IsNullOrEmpty(userId))
                {
                    return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.UserNotExist });
                }
                if ((MySeenWebApi.SyncModesApiData)mode == MySeenWebApi.SyncModesApiData.GetAll)
                {
                    var film = new List<MySeenWebApi.SyncJsonData>();
                    film.AddRange(ac.Films.Where(f => f.UserId == userId).Select(Map)
                        .Union(ac.Serials.Where(f => f.UserId == userId).Select(Map))
                        .Union(ac.Books.Where(f => f.UserId == userId).Select(Map)));

                    if (film.Any())
                    {
                        return Ok(film.AsEnumerable());
                    }
                    return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NoData });
                }
                return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.SomeErrorObtained });
        }

        [HttpPost]
        public IHttpActionResult Post()
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string userKey)
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string userKey, [FromUri]int mode)
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string userKey, [FromUri]int mode, [FromBody] IEnumerable<MySeenWebApi.SyncJsonData> data)
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NoLongerSupportedVersion });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string userKey, [FromUri]int mode, [FromUri]int apiVersion, [FromBody] IEnumerable<MySeenWebApi.SyncJsonData> data)
        {
            var logger = new NLogLogger();
            const string methodName = "public IHttpActionResult Get(string userKey, int mode, int apiVersion)";
            try
            {
                if (apiVersion != MySeenWebApi.ApiVersion)
                {
                    return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NoLongerSupportedVersion });
                }
                if (data == null || (MySeenWebApi.SyncModesApiData)mode != MySeenWebApi.SyncModesApiData.PostAll)
                {
                    return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
                }
                var userId = GetUserId(userKey);
                if (string.IsNullOrEmpty(userId))
                {
                    return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.UserNotExist });
                }
                var syncJsonDatas = data as MySeenWebApi.SyncJsonData[] ?? data.ToArray();
                if (syncJsonDatas.Any())
                {
                    var ac = new ApplicationDbContext();
                    foreach (var film in syncJsonDatas)
                    {
                        if (film.DataMode == (int)MySeenWebApi.DataModes.Film)
                        {
                            if (film.Id == null)
                            {
                                if (ac.Films.Any(f => f.Name == film.Name && f.UserId == userId))//с таким именем у нас уже есть
                                {
                                    var filmDb = ac.Films.First(f => f.Name == film.Name && f.UserId == userId);
                                    if (filmDb.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                                    {
                                        filmDb.DateChange = film.DateChange;
                                        filmDb.DateSee = film.DateSee;
                                        filmDb.Genre = film.Genre;
                                        filmDb.Rating = film.Rating;
                                    }
                                }
                                else //нету нового с таким именем
                                {
                                    ac.Films.Add(MapToFilm(film, userId));
                                }
                            }
                            else //старый обновился
                            {
                                if (ac.Films.Any(f => f.Id == film.Id && f.UserId == userId))//с таким ID есть в БД, обновим
                                {
                                    var filmDb = ac.Films.First(f => f.Id == film.Id && f.UserId == userId);
                                    if (filmDb.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                                    {
                                        filmDb.DateChange = film.DateChange;
                                        filmDb.DateSee = film.DateSee;
                                        filmDb.Genre = film.Genre;
                                        filmDb.Rating = film.Rating;
                                        filmDb.Name = film.Name;
                                    }
                                }
                                else
                                {
                                    ac.Films.Add(MapToFilm(film, userId));
                                }
                            }
                        }
                        else if (film.DataMode == (int)MySeenWebApi.DataModes.Serial)
                        {
                            if (film.Id == null)//Новый 
                            {
                                if (ac.Serials.Any(f => f.Name == film.Name && f.UserId == userId))//с таким именем у нас уже есть
                                {
                                    var filmDb = ac.Serials.First(f => f.Name == film.Name && f.UserId == userId);
                                    if (filmDb.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                                    {
                                        filmDb.DateChange = film.DateChange;
                                        filmDb.Genre = film.Genre;
                                        filmDb.Rating = film.Rating;
                                        filmDb.DateBegin = film.DateBegin;
                                        filmDb.DateLast = film.DateLast;
                                        filmDb.LastSeason = film.LastSeason;
                                        filmDb.LastSeries = film.LastSeries;
                                    }
                                }
                                else //нету нового с таким именем
                                {
                                    ac.Serials.Add(MapToSerial(film, userId));
                                }
                            }
                            else //старый обновился
                            {
                                if (ac.Serials.Any(f => f.Id == film.Id && f.UserId == userId))//с таким ID есть в БД, обновим
                                {
                                    var filmDb = ac.Serials.First(f => f.Id == film.Id && f.UserId == userId);
                                    if (filmDb.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                                    {
                                        filmDb.DateChange = film.DateChange;
                                        filmDb.Genre = film.Genre;
                                        filmDb.Rating = film.Rating;
                                        filmDb.DateBegin = film.DateBegin;
                                        filmDb.DateLast = film.DateLast;
                                        filmDb.LastSeason = film.LastSeason;
                                        filmDb.LastSeries = film.LastSeries;
                                        filmDb.Name = film.Name;
                                    }
                                }
                                else
                                {
                                    ac.Serials.Add(MapToSerial(film, userId));
                                }
                            }
                        }
                        else
                        {
                            if (film.Id == null)//Новый 
                            {
                                if (ac.Books.Any(f => f.Name == film.Name && f.UserId == userId))//с таким именем у нас уже есть
                                {
                                    var filmDb = ac.Books.First(f => f.Name == film.Name && f.UserId == userId);
                                    if (filmDb.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                                    {
                                        filmDb.DateChange = film.DateChange;
                                        filmDb.Genre = film.Genre;
                                        filmDb.Rating = film.Rating;
                                        filmDb.DateRead = film.DateRead;
                                        filmDb.Authors = film.Authors;
                                    }
                                }
                                else //нету нового с таким именем
                                {
                                    ac.Books.Add(MapToBook(film, userId));
                                }
                            }
                            else //старый обновился
                            {
                                if (ac.Books.Any(f => f.Id == film.Id && f.UserId == userId))//с таким ID есть в БД, обновим
                                {
                                    var filmDb = ac.Books.First(f => f.Id == film.Id && f.UserId == userId);
                                    if (filmDb.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                                    {
                                        filmDb.DateChange = film.DateChange;
                                        filmDb.Genre = film.Genre;
                                        filmDb.Rating = film.Rating;
                                        filmDb.DateRead = film.DateRead;
                                        filmDb.Authors = film.Authors;
                                        filmDb.Name = film.Name;
                                    }
                                }
                                else
                                {
                                    ac.Books.Add(MapToBook(film, userId));
                                }
                            }
                        }
                    }
                    ac.SaveChanges();
                    return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NewDataRecieved });
                }
                return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.SomeErrorObtained });
        }
    }
}
