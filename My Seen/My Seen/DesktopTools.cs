using System.Security.Cryptography;
using System.Data;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace My_Seen
{
    #region UnionResults
    public class SerialsResult : Serials
    {
        public SerialsResult()
        {

        }
        public SerialsResult(Serials s)
        {
            Id = s.Id;
            UsersId = s.UsersId;
            Name = s.Name;
            LastSeason = s.LastSeason;
            LastSeries = s.LastSeries;
            DateBegin = s.DateBegin;
            DateLast = s.DateLast;
            Rate = s.Rate;
        }
        public SerialsResult(Serials_New s)
        {
            Id = s.Id;
            UsersId = s.UsersId;
            Name = s.Name;
            LastSeason = s.LastSeason;
            LastSeries = s.LastSeries;
            DateBegin = s.DateBegin;
            DateLast = s.DateLast;
            Rate = s.Rate;
        }
        public SerialsResult(int _id, string _name, DateTime _dateBegin, string _rate,string _season,string _series)
        {
            Id = _id;
            Name = _name;
            DateBegin = _dateBegin;
            DateLast = DateTime.Now;
            try
            {
                Rate = Convert.ToInt32(_rate);
            }
            catch
            {
                Rate = 0;
            }
            try
            {
                LastSeason = Convert.ToInt32(_season);
            }
            catch
            {
                LastSeason = 1;
            }
            try
            {
                LastSeries = Convert.ToInt32(_series);
            }
            catch
            {
                LastSeries = 1;
            }
        }
    }
    public class FilmsResult : Films
    {
        public FilmsResult()
        {

        }
        public FilmsResult(Films f)
        {
            Id = f.Id;
            UsersId = f.UsersId;
            Name = f.Name;
            DateSee = f.DateSee;
            Rate = f.Rate;
        }
        public FilmsResult(Films_New f)
        {
            Id = f.Id;
            UsersId = f.UsersId;
            Name = f.Name;
            DateSee = f.DateSee;
            Rate = f.Rate;
        }
        public FilmsResult(int _id, string _name, DateTime _dateSee, string _rate)
        {
            Id = _id;
            Name = _name;
            DateSee = _dateSee;
            try
            {
                Rate = Convert.ToInt32(_rate);
            }
            catch
            {
                Rate = 0;
            }
        }
    }
    #endregion

    #region CultureTool
    public static class CultureInfoTool
    {
        public static class Cultures
        {
            private static string eng="en";
            private static string rus = "ru";
            public static string English
            {
                get
                {
                    return eng;
                }
            }
            public static string Russian
            {
                get
                {
                    return rus;
                }
            }
        }
        public static string GetCulture()
        {
            return Thread.CurrentThread.CurrentUICulture.ToString();
        }
        public static bool SetCulture(string cult)
        {
            if (GetCulture() != cult)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cult);
                return true;
            }
            return false;
        }

    }
    #endregion

    #region MD5Tools
    public static class MD5Tools
    {
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
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