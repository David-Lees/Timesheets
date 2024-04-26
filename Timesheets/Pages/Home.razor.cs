namespace Timesheets.Pages;

public partial class Home
{
    private readonly List<List<DateOnly>> dates = [];

    protected override void OnInitialized()
    {
        var startdate = new DateOnly(2024, 1, 1);
        for (int y = 0; y < 20; y++)
        {
            var year = new List<DateOnly>();
            for (int i = 0; i < 13; i++)
            {
                year.Add(startdate);
                startdate = startdate.AddDays(28);
            }
            dates.Add(year);
        }
    }
}