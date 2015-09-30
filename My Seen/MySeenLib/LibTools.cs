using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    message = "Too Short UserName";
                    return false;
                }
                return true;
            }
            public static bool ValidateEmail(ref string message, string email)
            {
                if (!email.Contains("@") || !email.Contains("."))
                {
                    message = "Email incorrect";
                    return false;
                }
                return true;
            }
            public static bool ValidatePassword(ref string message, string password, string passwordConfirm)
            {
                if (password != passwordConfirm)
                {
                    message = "Password and repeat not equal";
                    return false;
                }
                if (password.Length < 6)
                {
                    message = "Password Length < 6";
                    return false;
                }
                if (password.Contains("0") || password.Contains("1") || password.Contains("2") || password.Contains("3") || password.Contains("4") || password.Contains("5") || password.Contains("6") || password.Contains("7") || password.Contains("8") || password.Contains("9"))
                {
                    //Потом мож какие другие контроли
                }
                else
                {
                    message = "Password NOT Contains Digit";
                    return false;
                }
                return true;
            }
        }
    }
}
