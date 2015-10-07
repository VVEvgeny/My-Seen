using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Globalization;
using System.Threading;

namespace MySeenLib
{
    public static class LibTools
    {
        public static class Validation
        {
            public static bool ValidateUserName(ref string message, string userName)
            {
                if (userName.Length < 5)
                {
                    message = Resource.ShortUserName;
                    return false;
                }
                return true;
            }
            public static bool ValidateEmail(ref string message, string email)
            {
                if (!email.Contains("@") || !email.Contains("."))
                {
                    message = Resource.EmailIncorrect;
                    return false;
                }
                return true;
            }
            public static bool ValidatePassword(ref string message, string password, string passwordConfirm)
            {
                if (password != passwordConfirm)
                {
                    message = Resource.PasswordsNotEqual;
                    return false;
                }
                if (password.Length < 6)
                {
                    message = Resource.PasswordLength;
                    return false;
                }
                if (password.Contains("0") || password.Contains("1") || password.Contains("2") || password.Contains("3") || password.Contains("4") || password.Contains("5") || password.Contains("6") || password.Contains("7") || password.Contains("8") || password.Contains("9"))
                {
                    //Потом мож какие другие контроли
                }
                else
                {
                    message = Resource.PasswordNOTContainsDigit;
                    return false;
                }
                return true;
            }
        }
        public abstract class ListStringBase
        {
            public abstract void Load();
            public List<string> All;
            public List<string> GetAll()
            {
                if (All == null) Load();
                return All;
            }

            public int GetId(string _str)
            {
                if (All == null) Load();
                return All.IndexOf(_str);
            }
            public string GetById(int _id)
            {
                if (All == null) Load();
                if (_id >= All.Count) return "";
                return All[_id];
            }
            public int GetMaxId()
            {
                if (All == null) Load();
                return All.Count - 1;
            }
            public string GetMaxValue()
            {
                if (All == null) Load();
                if (All.Count==0) return "";
                return All[GetMaxId()];
            }
        }
        public class GenresBase : ListStringBase
        {
            public override void Load()
            {
                if (All == null)
                {
                    All = new List<string>();
                    All.Add(Resource.GenreThriller);
                    All.Add(Resource.GenreDocumentary);
                    All.Add(Resource.GenreDrama);
                    All.Add(Resource.GenreComedy);
                    All.Add(Resource.GenreConcert);
                    All.Add(Resource.GenreCartoon);
                    All.Add(Resource.GenreHorror);
                    All.Add(Resource.GenreFantastic);
                }
            }
        }
        public class RatingsBase : ListStringBase
        {
            public override void Load()
            {
                if (All == null)
                {
                    All = new List<string>();
                    All.Add("1");
                    All.Add("2");
                    All.Add("3");
                    All.Add("4");
                    All.Add("5");
                    All.Add("6");
                    All.Add("7");
                    All.Add("8");
                    All.Add("9");
                    All.Add("10");
                }
            }
        }

        public static GenresBase Genres = new GenresBase();
        public static RatingsBase Ratings = new RatingsBase();
    }
}
