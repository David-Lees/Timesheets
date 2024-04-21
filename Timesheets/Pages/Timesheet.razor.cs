using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Timesheets.Models;

namespace Timesheets.Pages;

public partial class Timesheet
{
    [Inject]
    public IConfiguration Config { get; set; } = default!;

    [Parameter]
    public string StartDate { get; set; } = string.Empty;

    private DateOnly _startDate;
    private BlobServiceClient? _client;
    private BlobContainerClient? _container;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Update();
    }

    public async Task<PeriodRecord?> LoadFile(DateOnly date)
    {
        if (_container is null) return null;
        var file = _container.GetBlobClient(date.ToString("yyyy-MM-dd") + ".json");
        using Stream stream = await file
           .OpenReadAsync()
           .ConfigureAwait(false);

        return await JsonSerializer.DeserializeAsync<PeriodRecord>(stream).ConfigureAwait(false);
    }

    public async Task SaveFile()
    {
        if (_container is null) return;
        var file = _container.GetBlobClient(_startDate.ToString("yyyy-MM-dd") + ".json");
        using MemoryStream ms = new();
        await JsonSerializer.SerializeAsync(ms, Period);
        ms.Position = 0;
        await file.UploadAsync(ms);
    }

    public async Task LoadSheet()
    {
        var accountUri = new Uri(Config.GetValue<string>("BlobStorage")!);
        _client = new BlobServiceClient(accountUri, new DefaultAzureCredential());
        _startDate = DateOnly.ParseExact(StartDate, "yyyy-MM-dd", new CultureInfo("en-GB"));
        _container = _client.GetBlobContainerClient("timesheets");

        Period = await LoadFile(_startDate);
        Period ??= new PeriodRecord(_startDate);

        if (_startDate == new DateOnly(2024, 1,1))
        {
            Period.PreviousCreditDebit = 0; // TODO: start time here
        }
        else
        {
            var previous = await LoadFile(_startDate.AddDays(28)) ?? new();
            Period.PreviousCreditDebit += previous.NextCreditDebit;
        }
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
    }

}