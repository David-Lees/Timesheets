namespace Timesheets.Models;

public class WeekRecord
{
    public DayRecord Monday { get; set; } = new();
    public DayRecord Tuesday { get; set; } = new();
    public DayRecord Wednesday { get; set; } = new();
    public DayRecord Thursday { get; set; } = new();
    public DayRecord Friday { get; set; } = new();
    public DayRecord Saturday { get; set; } = new();
    public DayRecord Sunday { get; set; } = new();

    public decimal WeekTotal =>
        Monday.DailyTotal +
        Tuesday.DailyTotal +
        Wednesday.DailyTotal +
        Thursday.DailyTotal +
        Friday.DailyTotal +
        Saturday.DailyTotal +
        Sunday.DailyTotal;

    public decimal ConditionedHours =>
        (Monday.Holiday ? 0 : Monday.ConditionedHours) +
        (Tuesday.Holiday ? 0 : Tuesday.ConditionedHours) +
        (Wednesday.Holiday ? 0 : Wednesday.ConditionedHours) +
        (Thursday.Holiday ? 0 : Thursday.ConditionedHours) +
        (Friday.Holiday ? 0 : Friday.ConditionedHours);

    public decimal HoursInOffice =>
        (Monday.InOffice ? Monday.DailyTotal : 0) +
        (Tuesday.InOffice ? Tuesday.DailyTotal : 0) +
        (Wednesday.InOffice ? Wednesday.DailyTotal : 0) +
        (Thursday.InOffice ? Thursday.DailyTotal : 0) +
        (Friday.InOffice ? Friday.DailyTotal : 0) +
        (Saturday.InOffice ? Saturday.DailyTotal : 0) +
        (Sunday.InOffice ? Sunday.DailyTotal : 0);

    public decimal HoursLessHoliday =>
        (!Monday.Holiday ? Monday.DailyTotal : 0) +
        (!Tuesday.Holiday ? Tuesday.DailyTotal : 0) +
        (!Wednesday.Holiday ? Wednesday.DailyTotal : 0) +
        (!Thursday.Holiday ? Thursday.DailyTotal : 0) +
        (!Friday.Holiday ? Friday.DailyTotal : 0) +
        (!Saturday.Holiday ? Saturday.DailyTotal : 0) +
        (!Sunday.Holiday ? Sunday.DailyTotal : 0);

    public WeekRecord()
    {
    }

    public WeekRecord(DateOnly StartDate)
    {
        Monday.Date = StartDate;
        Tuesday.Date = StartDate.AddDays(1);
        Wednesday.Date = StartDate.AddDays(2);
        Thursday.Date = StartDate.AddDays(3);
        Friday.Date = StartDate.AddDays(4);
        Saturday.Date = StartDate.AddDays(5);
        Sunday.Date = StartDate.AddDays(6);

        Monday.ConditionedHours = 7.4M;
        Tuesday.ConditionedHours = 7.4M;
        Wednesday.ConditionedHours = 7.4M;
        Thursday.ConditionedHours = 7.4M;
        Friday.ConditionedHours = 7.4M;
    }
}