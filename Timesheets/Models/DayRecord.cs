namespace Timesheets.Models;

public class DayRecord
{
    public DateOnly Date { get; set; }
    public string Start1Text { get; set; } = string.Empty;
    public decimal Start1 { get; set; }
    public string End1Text { get; set; } = string.Empty;
    public decimal End1 { get; set; }
    public string Start2Text { get; set; } = string.Empty;
    public decimal Start2 { get; set; }
    public string End2Text { get; set; } = string.Empty;
    public decimal End2 { get; set; }

    public string Reason { get; set; } = string.Empty;
    public string OtherText { get; set; } = string.Empty;
    public decimal Other { get; set; }
    public decimal DailyTotal { get; set; }
    public decimal ConditionedHours { get; set; }
    public decimal CumulativeTotal { get; set; }
    public bool InOffice { get; set; }
    public bool Holiday { get; set; }

    public decimal CumulativeHours => DailyTotal - ConditionedHours;
}