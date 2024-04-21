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
        Week1 = new(StartDate);
        Week2 = new(StartDate.AddDays(7));
        Week3 = new(StartDate.AddDays(14));
        Week4 = new(StartDate.AddDays(21));
    }

    public decimal ConditonedHoursLessHoliday =>
        Week1.ConditionedHours +
        Week2.ConditionedHours +
        Week3.ConditionedHours +
        Week4.ConditionedHours;

    public decimal HoursLessHoliday =>
        Week1.HoursLessHoliday +
        Week2.HoursLessHoliday +
        Week3.HoursLessHoliday +
        Week4.HoursLessHoliday;

    public decimal InOffice =>
        Week1.HoursInOffice +
        Week2.HoursInOffice +
        Week3.HoursInOffice +
        Week4.HoursInOffice;

    public decimal InOfficePercent => InOffice / ConditonedHoursLessHoliday * 100;
}