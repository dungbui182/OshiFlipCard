using System;

namespace GrandDreams.Core.Utilities
{
    public static class DateTimeExtensions
    {
        public static void GetCurrentQuarter(this DateTime dateTime, out int startTimeStamp, out int endTimeStamp)
        {
            startTimeStamp = 0;
            endTimeStamp = 0;

            int month = dateTime.Month;
            int year = dateTime.Year;
            
            switch (month)
            {
                case 1:
                case 2:
                case 3:
                    startTimeStamp = new DateTime(year, 1, 1).ToTimeStamp();
                    endTimeStamp = new DateTime(year, 3, 31).ToTimeStamp() + 86399;
                    break;
                case 4:
                case 5:
                case 6:
                    startTimeStamp = new DateTime(year, 4, 1).ToTimeStamp();
                    endTimeStamp = new DateTime(year, 6, 30).ToTimeStamp() + 86399;
                    break;
                case 7:
                case 8:
                case 9:
                    startTimeStamp = new DateTime(year, 7, 1).ToTimeStamp();
                    endTimeStamp = new DateTime(year, 9, 30).ToTimeStamp() + 86399;
                    break;
                case 10:
                case 11:
                case 12:
                    startTimeStamp = new DateTime(year, 10, 1).ToTimeStamp();
                    endTimeStamp = new DateTime(year, 12, 31).ToTimeStamp() + 86399;
                    break;
            }
        }

        public static bool TryParseToTimeStamp(this string stringValue, out Int32 timeStamp)
        {
            DateTime dateTime = DateTime.Now;
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            if (!DateTime.TryParseExact(stringValue, "dd/MM/yyyy", enUS, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                timeStamp = 0;
                return false;
            }

            timeStamp = (Int32)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return true;
        }

        public static bool TryParseToTimeStamp(this string stringValue, string format, out Int32 timeStamp)
        {
            DateTime dateTime = DateTime.Now;
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            if (!DateTime.TryParseExact(stringValue, format, enUS, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                timeStamp = 0;
                return false;
            }

            timeStamp = (Int32)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return true;
        }

        public static Int32 ToTimeStamp(this DateTime dateTime)
        {
            return (Int32)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime ConvertToDateTime(this int timeStamp)
        {
            System.DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTime = dateTime.AddSeconds(timeStamp).ToLocalTime();
            return dateTime;
        }

        public static DateTime ConvertToDateTime(this long timeStamp)
        {
            System.DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTime = dateTime.AddSeconds(timeStamp).ToLocalTime();
            return dateTime;
        }

        public static DateTime ConvertToDateTime(this string timeStampString)
        {
            int timeStamp = 0;
            int.TryParse(timeStampString, out timeStamp);
            System.DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTime = dateTime.AddSeconds(timeStamp).ToLocalTime();
            return dateTime;
        }

        public static DateTime GetBeginOfDay(this DateTime dateTime)
        {
            return DateTime.ParseExact(dateTime.GetDateString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DateTime GetEndOfDay(this DateTime dateTime)
        {
            return dateTime.GetBeginOfDay().AddSeconds(86399);
        }

        public static Int32 GetTimeStampBeginOfDay(this DateTime dateTime)
        {
            DateTime beginOfDay = DateTime.ParseExact(dateTime.GetDateString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            return beginOfDay.ToTimeStamp();
        }

        public static string ToTimeStampString(this DateTime dateTime)
        {
            return (dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
        }

        public static string GetDateString(this string timeStampString, string dateFormat = "dd/MM/yyyy")
        {
            return timeStampString.ConvertToDateTime().ToString(dateFormat);
        }

        public static string GetDateString(this DateTime date, string dateFormat = "dd/MM/yy")
        {
            return date.ToString(dateFormat);
        }

        public static string GetTimeString(this DateTime date, string timeFormat = "HH:mm:ss")
        {
            return date.ToString(timeFormat);
        }

        public static string GetDateTimeString(this DateTime date, string stringFormat = "HH:mm:ss dd/MM/yyyy")
        {
            return date.ToString(stringFormat);
        }
    }
}