using System.Globalization;

namespace Service.Utils;

public class TimeUtils
{
    // Get current week number (UTC)
    public static int GetCurrentWeekNumber()
    {
        var culture = CultureInfo.CurrentCulture;
        return culture.Calendar.GetWeekOfYear(DateTime.UtcNow, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    public static DateTime GetStartOfWeek(int year, int weekNumber)
    {
        DateTime firstDayOfYear = new DateTime(year, 1, 1);
        int offsetDays = DayOfWeek.Monday - firstDayOfYear.DayOfWeek;
        if (offsetDays > 0) offsetDays -= 7;

        DateTime firstWeekMonday = firstDayOfYear.AddDays(offsetDays);
        DateTime startOfWeek = firstWeekMonday.AddDays((weekNumber - 1) * 7);

        // Ensure the result is in UTC
        return startOfWeek.ToUniversalTime();
    }

    public static DateTime GetEndOfWeek(int year, int weekNumber)
    {
        // Get the start of the week (Monday)
        DateTime startOfWeek = GetStartOfWeek(year, weekNumber);

        // End of the week is Sunday, which is 6 days after Monday
        DateTime endOfWeek = startOfWeek.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);

        // Ensure the result is in UTC
        return endOfWeek.ToUniversalTime();
    }
    
    
    public static DateTime RegisterCloseDate(int year, int weekNumber)
    {
        // Get the start of the week (Monday)
        DateTime startOfWeek = GetStartOfWeek(year, weekNumber);

        // Calculate the Saturday of that week (Saturday is 5 days after Monday)
        DateTime saturday = startOfWeek.AddDays(5);

        // Set the time to 5:00 PM (17:00)
        DateTime saturdayAtFive = new DateTime(saturday.Year, saturday.Month, saturday.Day, 17, 0, 0);

        // Ensure the result is in UTC
        return saturdayAtFive.ToUniversalTime();
    }
    
    // Convert CET (Central European Time) to UTC
    public static DateTime ToUtc(DateTime cetDateTime)
    {
        // Define the CET time zone
        TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

        // Convert CET to UTC
        DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(cetDateTime, cetTimeZone);
        return utcDateTime;
    }

    // Convert UTC to CET (Central European Time)
    public static DateTime ToCet(DateTime utcDateTime)
    {
        // Define the CET time zone
        TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

        // Convert UTC to CET
        DateTime cetDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, cetTimeZone);
        return cetDateTime;
    }
    
    public static (int year, int weekNumber) GetNextWeek(int year, int weekNumber)
    {
        // Check if the given week number is the last week of the year (week 52 or 53)
        int daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
        DateTime lastDayOfYear = new DateTime(year, 12, 31);
        int lastWeekNumber = ISOWeek.GetWeekOfYear(lastDayOfYear);

        if (weekNumber == lastWeekNumber)
        {
            // If it's the last week, the next week will be the first week of the next year
            return (year + 1, 1);
        }

        // Otherwise, simply increment the week number
        return (year, weekNumber + 1);
    }
}


