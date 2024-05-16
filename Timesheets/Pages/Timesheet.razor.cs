using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Globalization;
using System.Net.Http.Json;
using Timesheets.Models;

namespace Timesheets.Pages;

public partial class Timesheet: IDisposable
{
    private HttpClient? _http;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IConfiguration Config { get; set; } = default!;

    [Inject]
    public IAccessTokenProviderAccessor Accessor { get; set; } = default!;

    [Inject]
    public IHttpClientFactory HttpClientFactory { get; set; } = default!;

    [Parameter]
    public string StartDate { get; set; } = string.Empty;

    private DateOnly _startDate;

    private IDisposable? registration;
    private bool disposedValue;
    private bool unsaved = false;
    private bool loaded = false;

    private ValueTask LocationChangingHandler(LocationChangingContext arg)
    {
        Console.WriteLine("Location is changing...");

        if (unsaved)
            arg.PreventNavigation();

        return ValueTask.CompletedTask;
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            registration = NavigationManager.RegisterLocationChangingHandler(LocationChangingHandler);
        }
        return base.OnAfterRenderAsync(firstRender);
    }

    protected override async Task OnInitializedAsync()
    {
        _http = HttpClientFactory.CreateClient("api");
        await LoadSheet();
        Update();
        StateHasChanged();
    }

    public async Task<PeriodRecord?> LoadFile(DateOnly date)
    {
        if (_http is null) return null;

        var response = await _http.GetAsync
            ($"/api/LoadFile?file={date.ToString("yyyy-MM-dd") + ".json"}&r={DateTime.Now.Ticks}") ?? throw new NotImplementedException("response is null");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PeriodRecord>();
    }

    public async Task SaveFile()
    {
        if (_http is null) return;
        await _http.PostAsJsonAsync($"/api/SaveFile?file={_startDate.ToString("yyyy-MM-dd") + ".json"}", Period);
        unsaved = false;
    }

    public async Task LoadSheet()
    {
        _startDate = DateOnly.ParseExact(StartDate, "yyyy-MM-dd", new CultureInfo("en-GB"));

        Period = await LoadFile(_startDate);
        if (Period is null) throw new NotImplementedException("Unable to load period");

        if (_startDate == new DateOnly(2024, 1, 1))
        {
            Period.PreviousCreditDebit = -18.95M;
        }
        else
        {
            var previous = await LoadFile(_startDate.AddDays(28)) ?? new();
            Period.PreviousCreditDebit += previous.NextCreditDebit;
        }

        loaded = true;
    }

    public PeriodRecord? Period { get; set; }

    public void Update()
    {
        if (Period is null) return;
        Console.WriteLine("Update in Timesheet called");
        Period.Week1.Monday.CumulativeTotal = Period.PreviousCreditDebit + Period.Week1.Monday.CumulativeHours;
        Period.Week1.Tuesday.CumulativeTotal = Period.Week1.Monday.CumulativeTotal + Period.Week1.Tuesday.CumulativeHours;
        Period.Week1.Wednesday.CumulativeTotal = Period.Week1.Tuesday.CumulativeTotal + Period.Week1.Wednesday.CumulativeHours;
        Period.Week1.Thursday.CumulativeTotal = Period.Week1.Wednesday.CumulativeTotal + Period.Week1.Thursday.CumulativeHours;
        Period.Week1.Friday.CumulativeTotal = Period.Week1.Thursday.CumulativeTotal + Period.Week1.Friday.CumulativeHours;
        Period.Week1.Saturday.CumulativeTotal = Period.Week1.Friday.CumulativeTotal + Period.Week1.Saturday.CumulativeHours;
        Period.Week1.Sunday.CumulativeTotal = Period.Week1.Saturday.CumulativeTotal + Period.Week1.Sunday.CumulativeHours;
        Period.Week2.Monday.CumulativeTotal = Period.Week1.Sunday.CumulativeTotal + Period.Week2.Monday.CumulativeHours;
        Period.Week2.Tuesday.CumulativeTotal = Period.Week2.Monday.CumulativeTotal + Period.Week2.Tuesday.CumulativeHours;
        Period.Week2.Wednesday.CumulativeTotal = Period.Week2.Tuesday.CumulativeTotal + Period.Week2.Wednesday.CumulativeHours;
        Period.Week2.Thursday.CumulativeTotal = Period.Week2.Wednesday.CumulativeTotal + Period.Week2.Thursday.CumulativeHours;
        Period.Week2.Friday.CumulativeTotal = Period.Week2.Thursday.CumulativeTotal + Period.Week2.Friday.CumulativeHours;
        Period.Week2.Saturday.CumulativeTotal = Period.Week2.Friday.CumulativeTotal + Period.Week2.Saturday.CumulativeHours;
        Period.Week2.Sunday.CumulativeTotal = Period.Week2.Saturday.CumulativeTotal + Period.Week2.Sunday.CumulativeHours;
        Period.Week3.Monday.CumulativeTotal = Period.Week2.Sunday.CumulativeTotal + Period.Week3.Monday.CumulativeHours;
        Period.Week3.Tuesday.CumulativeTotal = Period.Week3.Monday.CumulativeTotal + Period.Week3.Tuesday.CumulativeHours;
        Period.Week3.Wednesday.CumulativeTotal = Period.Week3.Tuesday.CumulativeTotal + Period.Week3.Wednesday.CumulativeHours;
        Period.Week3.Thursday.CumulativeTotal = Period.Week3.Wednesday.CumulativeTotal + Period.Week3.Thursday.CumulativeHours;
        Period.Week3.Friday.CumulativeTotal = Period.Week3.Thursday.CumulativeTotal + Period.Week3.Friday.CumulativeHours;
        Period.Week3.Saturday.CumulativeTotal = Period.Week3.Friday.CumulativeTotal + Period.Week3.Saturday.CumulativeHours;
        Period.Week3.Sunday.CumulativeTotal = Period.Week3.Saturday.CumulativeTotal + Period.Week3.Sunday.CumulativeHours;
        Period.Week4.Monday.CumulativeTotal = Period.Week3.Sunday.CumulativeTotal + Period.Week4.Monday.CumulativeHours;
        Period.Week4.Tuesday.CumulativeTotal = Period.Week4.Monday.CumulativeTotal + Period.Week4.Tuesday.CumulativeHours;
        Period.Week4.Wednesday.CumulativeTotal = Period.Week4.Tuesday.CumulativeTotal + Period.Week4.Wednesday.CumulativeHours;
        Period.Week4.Thursday.CumulativeTotal = Period.Week4.Wednesday.CumulativeTotal + Period.Week4.Thursday.CumulativeHours;
        Period.Week4.Friday.CumulativeTotal = Period.Week4.Thursday.CumulativeTotal + Period.Week4.Friday.CumulativeHours;
        Period.Week4.Saturday.CumulativeTotal = Period.Week4.Friday.CumulativeTotal + Period.Week4.Saturday.CumulativeHours;
        Period.Week4.Sunday.CumulativeTotal = Period.Week4.Saturday.CumulativeTotal + Period.Week4.Sunday.CumulativeHours;

        Period.NextCreditDebit = Period.Week4.Sunday.CumulativeTotal;
        unsaved = true;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                registration?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}