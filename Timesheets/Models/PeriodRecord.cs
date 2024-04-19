namespace Timesheets.Models;

public class PeriodRecord
{
    public DateOnly StartDate { get; set; }
    public WeekRecord Week1 { get; set; } = new();
    public WeekRecord Week2 { get; set; } = new();
    public WeekRecord Week3 { get; set; } = new();
    public WeekRecord Week4 { get; set; } = new();

    public decimal PreviousCreditDebit { get; set; }
    public decimal NextCreditDebit { get; set; }

    public PeriodRecord()
    {
    }

    public PeriodRecord(DateOnly startdate)
    {
        StartDate = startdate;
        Week1 = new(StartDate, PreviousCreditDebit);
        Week2 = new(StartDate, 0);
        Week3 = new(StartDate, 0);
        Week4 = new(StartDate, 0);
    }
}