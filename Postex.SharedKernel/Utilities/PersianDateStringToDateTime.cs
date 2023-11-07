using System;
using System.Globalization;

namespace Postex.SharedKernel.Utilities
{
    public static class DateTimeExtensions
    {
        public static DateTime PersianDateStringToDateTime(this string persianDate)
        {
            PersianCalendar pc = new();

            var persianDateSplitedParts = persianDate.Split(" ")[0].Split('/');
            if (persianDate.Contains(":"))
            {
                var persianTimeSplitedParts = persianDate.Split(" ")[1].Split(':');
                DateTime dateTime = new(int.Parse(persianDateSplitedParts[0]), int.Parse(persianDateSplitedParts[1]), int.Parse(persianDateSplitedParts[2]), int.Parse(persianTimeSplitedParts[0]), int.Parse(persianTimeSplitedParts[1]), 0, pc);
                return DateTime.Parse(dateTime.ToString(CultureInfo.CreateSpecificCulture("en-US")));
            }
            else
            {
                DateTime dateTime = new(int.Parse(persianDateSplitedParts[0]), int.Parse(persianDateSplitedParts[1]), int.Parse(persianDateSplitedParts[2]), pc);
                return DateTime.Parse(dateTime.ToString(CultureInfo.CreateSpecificCulture("en-US")));
            }
        }
        public static string ToPersianDateTime(this DateTime date)
        {
            if (date == null)
                return "";
            PersianCalendar pc = new();
            var result = $"{pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)} : {pc.GetHour(date)}:{pc.GetMinute(date)}:{pc.GetMilliseconds(date)}";
            return result;
        }
        public static string ToPersianDateTime(this DateTime date, string Seperator = "/")
        {
            if (date == null)
                return "";
            PersianCalendar pc = new();
            var result = $"{pc.GetYear(date)}{Seperator}{pc.GetMonth(date)}{Seperator}{pc.GetDayOfMonth(date)} : {pc.GetHour(date)}:{pc.GetMinute(date)}:{pc.GetMilliseconds(date)}";
            return result;
        }
        public static string ToPersianDate(this DateTime date)
        {
            if (date == null)
                return "";
            PersianCalendar pc = new();
            var result = $"{pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)}";
            return result;
        }
        public static string ToPersianDate(this DateTime date, string Seperator = "/")
        {
            if (date == null)
                return "";
            PersianCalendar pc = new();
            var result = $"{pc.GetYear(date)}{Seperator}{pc.GetMonth(date)}{Seperator}{pc.GetDayOfMonth(date)}";
            return result;
        }
    }
}
