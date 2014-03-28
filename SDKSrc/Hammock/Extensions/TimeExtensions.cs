using System;
using System.Globalization;

namespace TencentWeiboSDK.Hammock.Extensions
{
    public static class TimeExtensions
    {
        public static DateTime FromNow(this TimeSpan value)
        {
            return new DateTime((DateTime.Now + value).Ticks);
        }

        public static DateTime FromUnixTime(this long seconds)
        {
            var time = new DateTime(1970, 1, 1);
            time = time.AddSeconds(seconds);

            return time.ToLocalTime();
        }

        public static long ToUnixTime(this DateTime dateTime)
        {
            var timeSpan = (dateTime - new DateTime(1970, 1, 1));
            var timestamp = (long)timeSpan.TotalSeconds;
            
            return timestamp;
        }

        public static long ToUnixTime(this long datetime)
        {
            try
            {
                string[] strArray = datetime.ToString().Split(new char[] { ' ' });
                return DateTime.ParseExact(string.Join(" ", strArray), "yyyyMMddHHmmss", CultureInfo.CurrentCulture).ToUniversalTime().ToUnixTime();
            }
            catch
            {
                return -1;
            }
        }
    }
}