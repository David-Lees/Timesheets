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

    public decimal CumaltiveTotal =>
        Monday.CumulativeTotal +
        Tuesday.CumulativeTotal +
        Wednesday.CumulativeTotal +
        Thursday.CumulativeTotal +
        Friday.CumulativeTotal +
        Saturday.CumulativeTotal +
        Sunday.CumulativeTotal;

    public WeekRecord()
    {
    }

    public WeekRecord(DateOnly StartDate, decimal previousCreditDebit)
    {
        Monday.Date = StartDate;
        Tuesday.Date = StartDate.AddDays(1);
        Wednesday.Date = StartDate.AddDays(2);
        Thursday.Date = StartDate.AddDays(3);
        Friday.Date = StartDate.AddDays(4);
        Saturday.Date = StartDate.AddDays(5);
        Sunday.Date = StartDate.AddDays(6);
    }
}