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
    public delegate void MySeenEventHandler();
    public class MySeenEvent
    {
        public event MySeenEventHandler Event;
        public void Exec()
        {
            if (Event != null) Event();
        }
    }

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