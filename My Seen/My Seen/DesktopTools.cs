using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using MySeenLib;
using System.IO;

namespace My_Seen
{
    public static class WebApi
    {
        public static string CheckUser(string email)
        {
            if (email.Length == 0)
            {
                return Resource.EnterEmail;
            }
            try
            {
                WebRequest req = WebRequest.Create(
                    MySeenWebApi.ApiHost + MySeenWebApi.ApiUsers + Md5Tools.GetMd5Hash(email.ToLower()) 
                    + "/" 
                    + ((int)MySeenWebApi.SyncModesApiUsers.IsUserExists).ToString()
                    + "/" + MySeenWebApi.ApiVersion.ToString()
                    );
                // ReSharper disable once RedundantArgumentName
                // ReSharper disable once AssignNullToNotNullAttribute
                MySeenWebApi.SyncJsonAnswer answer = MySeenWebApi.GetResponseAnswer((new StreamReader(stream: req.GetResponse().GetResponseStream())).ReadToEnd());
                if (answer != null)
                {
                    if (answer.Value != MySeenWebApi.SyncJsonAnswer.Values.Ok)
                    {
                        if (answer.Value == MySeenWebApi.SyncJsonAnswer.Values.UserNotExist)
                        {
                            return Resource.UserNotExist;
                        }
                        else if (answer.Value == MySeenWebApi.SyncJsonAnswer.Values.NoLongerSupportedVersion)
                        {
                            return Resource.NoLongerSupportedVersion;
                        }
                        else return answer.Value.ToString();
                    }
                    else
                    {
                        return Resource.UserOK;
                    }
                }
                req.GetResponse().Close();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return Resource.ApiError;
        }
        public static bool Sync(Users user)
        {
            ServicePointManager.Expect100Continue = false; 

            List<MySeenWebApi.SyncJsonData> films = new List<MySeenWebApi.SyncJsonData>();
            ModelContainer mc = new ModelContainer();
            //Буду отдавать ему ВСЁ, так надежнее
            films.AddRange(
                mc.FilmsSet.Where(f => f.UsersId == user.Id).Select(Map)
                .Union(mc.SerialsSet.Where(f => f.UsersId == user.Id).Select(Map))
                .Union(mc.BooksSet.Where(f => f.UsersId == user.Id).Select(Map))
                );
            WebRequest req;
            MySeenWebApi.SyncJsonAnswer answer;
            if (films.Any())
            {
                req = WebRequest.Create(
                    MySeenWebApi.ApiHost + MySeenWebApi.ApiSync + Md5Tools.GetMd5Hash(user.Email.ToLower()) 
                    + "/" + ((int)MySeenWebApi.SyncModesApiData.PostAll).ToString()
                    + "/" + MySeenWebApi.ApiVersion.ToString()
                    );
                req.Method = "POST";
                req.Credentials = CredentialCache.DefaultCredentials;
                ((HttpWebRequest)req).UserAgent = "MySeen";
                ((HttpWebRequest)req).ProtocolVersion = HttpVersion.Version10;//для прокси
                req.ContentType = "application/json";
                string postData = MySeenWebApi.SetResponse(films);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                // ReSharper disable once RedundantArgumentName
                // ReSharper disable once AssignNullToNotNullAttribute
                answer = MySeenWebApi.GetResponseAnswer((new StreamReader(stream: req.GetResponse().GetResponseStream())).ReadToEnd());
                req.GetResponse().Close();
                if (answer == null)
                {
                    MessageBox.Show(Resource.ApiError);
                    return false;
                }
            }

            req = WebRequest.Create(
                MySeenWebApi.ApiHost + MySeenWebApi.ApiSync + Md5Tools.GetMd5Hash(user.Email.ToLower()) 
                + "/" + ((int)MySeenWebApi.SyncModesApiData.GetAll).ToString()
                + "/" + MySeenWebApi.ApiVersion.ToString()
                );

            // ReSharper disable once RedundantArgumentName
            // ReSharper disable once AssignNullToNotNullAttribute
            string data = (new StreamReader(stream: req.GetResponse().GetResponseStream())).ReadToEnd();
            req.GetResponse().Close();

            answer = MySeenWebApi.GetResponseAnswer(data);

            if (answer != null)
            {
                if (answer.Value == MySeenWebApi.SyncJsonAnswer.Values.UserNotExist)
                {
                    MessageBox.Show(Resource.UserNotExist);
                }
                else if (answer.Value == MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode)
                {
                    MessageBox.Show(Resource.BadRequestMode);
                }
                else MessageBox.Show(answer.Value.ToString()); 
            }
            else
            {
                //Для 2х БД алгоритм хороший, но тут есть 3 БД, надо между всеми...
                mc.FilmsSet.RemoveRange(mc.FilmsSet.Where(f => f.UsersId == user.Id));
                mc.SerialsSet.RemoveRange(mc.SerialsSet.Where(f => f.UsersId == user.Id));
                mc.BooksSet.RemoveRange(mc.BooksSet.Where(f => f.UsersId == user.Id));
                mc.SaveChanges();
                foreach (MySeenWebApi.SyncJsonData film in MySeenWebApi.GetResponse(data))
                {
                    if (film.DataMode==(int)MySeenWebApi.DataModes.Film)
                    {
                        mc.FilmsSet.Add(MapToFilm(film, user.Id));
                    }
                    else if (film.DataMode == (int)MySeenWebApi.DataModes.Serial)
                    {
                        mc.SerialsSet.Add(MapToSerial(film, user.Id));
                    }
                    else
                    {
                        mc.BooksSet.Add(MapToBook(film, user.Id));
                    }
                }
            }
            mc.SaveChanges();
            MessageBox.Show(Resource.SyncOK);
            return true;//LoadItemsToListView();
        }
        public static MySeenWebApi.SyncJsonData Map(Films model)
        {
            if (model == null) return new MySeenWebApi.SyncJsonData();

            return new MySeenWebApi.SyncJsonData
            {
                DataMode = (int)MySeenWebApi.DataModes.Film,
                Id = model.Id_R,
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rating = model.Rating,
                IsDeleted = model.isDeleted
            };
        }
        public static MySeenWebApi.SyncJsonData Map(Serials model)
        {
            if (model == null) return new MySeenWebApi.SyncJsonData();

            return new MySeenWebApi.SyncJsonData
            {
                DataMode = (int)MySeenWebApi.DataModes.Serial,
                Id = model.Id_R,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                DateBegin = model.DateBegin,
                DateLast = model.DateLast,
                LastSeason = model.LastSeason,
                LastSeries = model.LastSeries,
                IsDeleted = model.isDeleted
            };
        }
        public static MySeenWebApi.SyncJsonData Map(Books model)
        {
            if (model == null) return new MySeenWebApi.SyncJsonData();

            return new MySeenWebApi.SyncJsonData
            {
                DataMode = (int)MySeenWebApi.DataModes.Book,
                Id = model.Id_R,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                IsDeleted = model.isDeleted, 
                Authors=model.Authors,
               DateRead=model.DateRead,
            };
        }
        public static Films MapToFilm(MySeenWebApi.SyncJsonData model, int userId)
        {
            if (model == null) return new Films();

            return new Films
            {
                Id_R = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rating = model.Rating,
                isDeleted = model.IsDeleted,
                UsersId = userId
            };
        }
        public static Serials MapToSerial(MySeenWebApi.SyncJsonData model, int userId)
        {
            if (model == null) return new Serials();

            return new Serials
            {
                Id_R = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                DateBegin = model.DateBegin,
                DateLast = model.DateLast,
                LastSeason = model.LastSeason,
                LastSeries = model.LastSeries,
                isDeleted = model.IsDeleted,
                UsersId = userId
            };
        }
        public static Books MapToBook(MySeenWebApi.SyncJsonData model, int userId)
        {
            if (model == null) return new Books();

            return new Books
            {
                Id_R = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                isDeleted = model.IsDeleted,
                UsersId = userId,
                Authors=model.Authors, 
                DateRead=model.DateRead
            };
        }
    }

    public delegate void MySeenEventHandler();
    public class MySeenEvent
    {
        public event MySeenEventHandler Event;
        public void Exec()
        {
            if (Event != null) Event();
        }
    }
    public static class ErrorProviderTools
    {
        public static bool IsValid(ErrorProvider errorProvider)
        {
            return errorProvider.ContainerControl.Controls.Cast<Control>().All(c => string.IsNullOrEmpty(errorProvider.GetError(c)));
        }
    }

    #region Md5Tools
    public static class Md5Tools
    {
        public static string GetMd5Hash(string input)
        {
            try
            {
                MD5 md5Hash = MD5.Create();
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                foreach (byte t in data)
                {
                    sBuilder.Append(t.ToString("x2"));
                }
                return sBuilder.ToString();
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }
        public static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion
}