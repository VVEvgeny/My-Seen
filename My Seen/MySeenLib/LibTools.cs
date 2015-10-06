using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Globalization;
using System.Threading;

namespace MySeenLib
{
    public static class Genres
    {
        private static List<string> All;
        
        public static int GetGenreId(string _genre)
        {
            if (All == null) GetAll();
            return All.IndexOf(_genre);
        }
        public static string GetGenreById(int _id)
        {
            if (All == null) GetAll();
            if (_id >= All.Count) return "";
            return All[_id];
        }

        public static List<string> GetAll()
        {
            if(All==null)
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
            return All;
        }

    }
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
    }
}
