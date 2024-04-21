namespace Timesheets.Models;

public static class TimeHelper
{
    public static string FormatTime(decimal d) => 
        $"{(d > 0 ? Math.Floor(d) : Math.Ceiling(d))}:{(int)Math.Abs(d % 1 * 60):d2}";
}
