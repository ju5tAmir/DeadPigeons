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

    // Get the start date of the current week in UTC
    public static DateTime GetCurrentWeekStartDate()
    {
        DateTime today = DateTime.UtcNow;
        int daysToMonday = ((int)today.DayOfWeek == 0 ? 7 : (int)today.DayOfWeek) - 1;
        DateTime startOfWeek = today.Date.AddDays(-daysToMonday);
            
        return startOfWeek;
    }

    // Get the end date of the current week in UTC
    public static DateTime GetCurrentWeekEndDate()
    {
        DateTime today = DateTime.UtcNow;
        int daysToMonday = ((int)today.DayOfWeek == 0 ? 7 : (int)today.DayOfWeek) - 1;
        DateTime startOfWeek = today.Date.AddDays(-daysToMonday);

        // End of week (Sunday at 23:59:59 UTC)
        DateTime endOfWeek = startOfWeek.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);

        return endOfWeek;
    }
}

