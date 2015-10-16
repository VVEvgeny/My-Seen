using System.Security.Cryptography;
using System.Data;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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
                WebRequest req = WebRequest.Create(MySeenWebApi.ApiHost + MySeenWebApi.ApiUsers + MD5Tools.GetMd5Hash(email.ToLower()) + "/" + ((int)MySeenWebApi.SyncModesApiUsers.isUserExists).ToString());
                MySeenWebApi.SyncJsonAnswer answer = MySeenWebApi.GetResponseAnswer((new StreamReader(req.GetResponse().GetResponseStream())).ReadToEnd());
                if (answer != null)
                {
                    if (answer.Value == MySeenWebApi.SyncJsonAnswer.Values.UserNotExist)
                    {
                        return Resource.UserNotExist;
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
        public static bool isValid(ErrorProvider errorProvider)
        {
          foreach (Control c in errorProvider.ContainerControl.Controls)
                if (!string.IsNullOrEmpty(errorProvider.GetError(c)))
                    return false;
            return true;
        }
    }

    #region MD5Tools
    public static class MD5Tools
    {
        public static string GetMd5Hash(string input)
        {
            try
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
            catch
            {

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