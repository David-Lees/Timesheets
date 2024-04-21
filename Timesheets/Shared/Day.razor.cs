using Microsoft.AspNetCore.Components;
using Timesheets.Models;

namespace Timesheets.Shared;

public partial class Day
{
    [Parameter]
    public DayRecord Item { get; set; } = default!;

    [Parameter]
    public EventCallback<DayRecord> ItemChanged { get; set; }

    [Parameter]
    public decimal PreviousTotal { get; set; }

    private string CssClass
    {
        get
        {
            var style =
                (Item?.Date.DayOfWeek == DayOfWeek.Monday ? "heavy-top " : " ") +
                (Item?.Date.DayOfWeek == DayOfWeek.Saturday ? "weekend " : " ") +
                (Item?.Date.DayOfWeek == DayOfWeek.Sunday ? "heavy-bottom weekend" : "");

            if (Item?.InOffice == true) { style += " in-office"; }
            if (Item?.Holiday == true) { style += " holiday"; }
            return style;
        }
    }

    public void TextChanged()
    {
        Console.WriteLine("TextChanged in Day called");
        Item ??= new();
        Item.DailyTotal = (Item.End1 - Item.Start1) + (Item.End2 - Item.Start2) + (Item.Other);
        ItemChanged.InvokeAsync(Item);
    }

    public string GetAmTotal() => Item is not null ? TimeHelper.FormatTime(Item.End1 - Item.Start1) : string.Empty;
    public string GetPmTotal() => Item is not null ? TimeHelper.FormatTime(Item.End2 - Item.Start2) : string.Empty;
}