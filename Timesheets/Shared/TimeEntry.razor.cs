using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text.RegularExpressions;

namespace Timesheets.Shared;

public partial class TimeEntry
{
    [Parameter]
    public string TextValue { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> TextValueChanged { get; set; }

    [Parameter]
    public decimal DecimalValue { get; set; }

    [Parameter]
    public EventCallback<decimal> DecimalValueChanged { get; set; }

    [Parameter]
    public EventCallback Updated { get; set; }

    public void Update()
    {
        Console.WriteLine("Update in TimeEntry called");
        var regex = TimeRegex();
        if (!regex.IsMatch(TextValue))
        {
            DecimalValue = 0;
            return;
        }
        var parts = TextValue.Split(':');
        if (parts.Length != 2)
        {
            DecimalValue = 0;
            return;
        }
        DecimalValue = Convert.ToDecimal(parts[0]) + (Convert.ToDecimal(parts[1]) / 60);
        DecimalValueChanged.InvokeAsync(DecimalValue);
        TextValueChanged.InvokeAsync(TextValue);
        Updated.InvokeAsync();
    }

    private bool PreventDefault = false;

    private void OnKeyDown(KeyboardEventArgs e)
    {
        var c = e.Key[0];
        PreventDefault = e.Key.Length == 1 && (char.IsLetter(e.Key[0]) || (char.IsPunctuation(c) && c != ':'));
    }

    [GeneratedRegex("\\d+:\\d+")]
    private static partial Regex TimeRegex();
}