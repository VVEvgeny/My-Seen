using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

public static class CookieStore
{
    public static void SetCookie(string key, string value)
    {
        HttpContext.Current.Request.Cookies.Remove(key);
        HttpCookie cookie = new HttpCookie(key);
        cookie.Expires = DateTime.Now.AddDays(1);
        cookie.Value = value;
        HttpContext.Current.Request.Cookies.Add(cookie);
    }
    public static string GetCookie(string key)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
        if (cookie != null)
        {
            return cookie.Value;
        }
        return string.Empty;
    }

}
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
