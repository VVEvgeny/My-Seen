using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;
using static MySeenLib.MySeenWebApi;

namespace MySeenWeb.Controllers.Api
{
    public class ApiSyncController : ApiController
    {
        public static SyncJsonData Map(Films model)
        {
            if (model == null) return new SyncJsonData();

            return new SyncJsonData
            {
                DataMode = (int)DataModes.Film,
                Id = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rating = model.Rating
            };
        }
        public static Films MapToFilm(SyncJsonData model, string userId)
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
        public static SyncJsonData Map(Serials model)
        {
            if (model == null) return new SyncJsonData();

            return new SyncJsonData
            {
                DataMode = (int)DataModes.Serial,
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
        public static Serials MapToSerial(SyncJsonData model, string userId)
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
        public static SyncJsonData Map(Books model)
        {
            if (model == null) return new SyncJsonData();

            return new SyncJsonData
            {
                DataMode = (int)DataModes.Book,
                Id = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                Authors = model.Authors,
                DateRead = model.DateRead
            };
        }
        public static Books MapToBook(SyncJsonData model, string userId)
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
            return ac.Users.Any(u => u.UniqueKey == userKey) ? ac.Users.First(u => u.UniqueKey == userKey).Id : string.Empty;
        }
        public IHttpActionResult Get()
        {
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
        }
        public IHttpActionResult Get(string userKey)
        {
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
        }
        public IHttpActionResult Get(string userKey, int mode)
        {
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.NoLongerSupportedVersion });
        }
        public IHttpActionResult Get(string userKey, int mode, int apiVersion)
        {
            var logger = new NLogLogger();
            const string methodName = "public IHttpActionResult Get(string userKey, int mode, int apiVersion)";
            try
            {
                if (apiVersion != ApiVersion)
                {
                    return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.NoLongerSupportedVersion });
                }

                var ac = new ApplicationDbContext();
                var userId = GetUserId(userKey);
                if (string.IsNullOrEmpty(userId))
                {
                    return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.UserNotExist });
                }
                if ((SyncModesApiData)mode == SyncModesApiData.GetAll)
                {
                    var film = new List<SyncJsonData>();
                    film.AddRange(ac.Films.Where(f => f.UserId == userId).Select(Map)
                        .Union(ac.Serials.Where(f => f.UserId == userId).Select(Map))
                        .Union(ac.Books.Where(f => f.UserId == userId).Select(Map)));

                    if (film.Any())
                    {
                        return Ok(film.AsEnumerable());
                    }
                    return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.NoData });
                }
                return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.SomeErrorObtained });
        }

        [HttpPost]
        public IHttpActionResult Post()
        {
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string userKey)
        {
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string userKey, [FromUri]int mode)
        {
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string userKey, [FromUri]int mode, [FromBody] IEnumerable<SyncJsonData> data)
        {
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.NoLongerSupportedVersion });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string userKey, [FromUri]int mode, [FromUri]int apiVersion, [FromBody] IEnumerable<SyncJsonData> data)
        {
            var logger = new NLogLogger();
            const string methodName = "public IHttpActionResult Get(string userKey, int mode, int apiVersion)";
            try
            {
                if (apiVersion != ApiVersion)
                {
                    return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.NoLongerSupportedVersion });
                }
                if (data == null || (SyncModesApiData)mode != SyncModesApiData.PostAll)
                {
                    return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
                }
                var userId = GetUserId(userKey);
                if (string.IsNullOrEmpty(userId))
                {
                    return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.UserNotExist });
                }
                var syncJsonDatas = data as SyncJsonData[] ?? data.ToArray();
                if (syncJsonDatas.Any())
                {
                    var ac = new ApplicationDbContext();
                    foreach (var film in syncJsonDatas)
                    {
                        if (film.DataMode == (int)DataModes.Film)
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
                        else if (film.DataMode == (int)DataModes.Serial)
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
                    return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.NewDataRecieved });
                }
                return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.SomeErrorObtained });
        }
    }
}
