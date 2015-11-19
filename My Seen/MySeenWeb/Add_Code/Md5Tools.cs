using System;
using System.Security.Cryptography;
using System.Text;

namespace MySeenWeb.Add_Code
{

    #region Md5Tools

    public static class Md5Tools
    {
        public static string GetMd5Hash(string input)
        {
            var md5Hash = MD5.Create();
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static bool VerifyMd5Hash(string input, string hash)
        {
            var hashOfInput = GetMd5Hash(input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }

    #endregion
}