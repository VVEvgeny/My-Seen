using System.Security.Cryptography;
using System.Data;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;

namespace My_Seen
{
    public class FilmsResult : Films
    {
        public FilmsResult()
        {

        }
        public FilmsResult(Films_New f)
        {
            Id = f.Id;
            UsersId = f.UsersId;
            Name = f.Name;
            DateSee = f.DateSee;
            Rate = f.Rate;
        }
        public FilmsResult(Films f)
        {
            Id = f.Id;
            UsersId = f.UsersId;
            Name = f.Name;
            DateSee = f.DateSee;
            Rate = f.Rate;
        }
        public FilmsResult(string _name, DateTime _dateSee, string _rate)
        {
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
}