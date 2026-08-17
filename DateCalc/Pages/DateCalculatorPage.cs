using System.Globalization;
using DateCalc.Core;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace DateCalc;

internal sealed partial class DateCalculatorPage : DynamicListPage
{
    private IListItem[] _items;

    public DateCalculatorPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "Date Calculator";
        Name = "Open";
        PlaceholderText = "today + 30d";
        EmptyContent = new CommandItem(new NoOpCommand())
        {
            Icon = Icon,
            Title = "Enter a date expression",
            Subtitle = "Examples: today + 30d, 2026-08-17 - 2w",
        };
        _items = [CreateHelpItem()];
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        var culture = CultureInfo.CurrentCulture;
        var today = DateOnly.FromDateTime(DateTime.Today);

        if (DateExpressionParser.TryCalculate(newSearch, culture, today, out var calculation, out var error))
        {
            var result = calculation.ResultDate.ToString("d", culture);
            var start = calculation.StartDate.ToString("d", culture);
            _items =
            [
                new ListItem(new CopyTextCommand(result))
                {
                    Icon = Icon,
                    Title = result,
                    Subtitle = $"{start}  |  Press Enter to copy",
                    TextToSuggest = result,
                },
            ];
        }
        else if (string.IsNullOrWhiteSpace(newSearch))
        {
            _items = [CreateHelpItem()];
        }
        else
        {
            _items =
            [
                new ListItem(new NoOpCommand())
                {
                    Icon = Icon,
                    Title = error,
                    Subtitle = "Examples: today + 30d, 17/08/2026 - 2w",
                },
            ];
        }

        RaiseItemsChanged(_items.Length);
    }

    public override IListItem[] GetItems() => _items;

    private ListItem CreateHelpItem() => new(new NoOpCommand())
    {
        Icon = Icon,
        Title = "Enter a starting date, operator, and duration",
        Subtitle = "Units: d = days, w = weeks, m = months, y = years",
    };
}
