using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MySeenWeb.Add_Code
{
    public enum BrowserTypes
    {
        Chrome = 1,
        Firefox,
        Opera,
        Safari,
        Ie,
        InternetExplorer
    }

    public static class ExternalNotOwinProviders
    {
        public static string Yandex = "Yandex";
    }

    #region Md5Tools
    public static class Md5Tools
    {
        public static string Generate(params object[] values)
        {
            var genkey = values.Aggregate(string.Empty, (current, el) => current + el);
            var r = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 20; i++)
            {
                genkey += r.Next().ToString();
            }
            genkey = Get(genkey);
            return genkey;
        }
        public static string Get(string input)
        {
            var md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public static bool Verify(string input, string hash)
        {
            var hashOfInput = Get(input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }
    #endregion
}