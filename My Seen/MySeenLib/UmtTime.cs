using System;

namespace MySeenLib
{
    public static class UmtTime
    {
        public static DateTime To(DateTime datetime)
        {
            return datetime.ToUniversalTime();
        }

        public static DateTime From(DateTime datetime)
        {
            return datetime.ToLocalTime();
        }
    }
}