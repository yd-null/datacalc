using System.Globalization;
using System.Text.RegularExpressions;

namespace DateCalc.Core;

public static partial class DateExpressionParser
{
    public static bool TryCalculate(
        string? expression,
        CultureInfo culture,
        DateOnly today,
        out DateCalculation calculation,
        out string error)
    {
        calculation = default;

        if (string.IsNullOrWhiteSpace(expression))
        {
            error = "Enter a date expression, for example: today + 30d";
            return false;
        }

        var match = ExpressionPattern().Match(expression);
        if (!match.Success)
        {
            error = "Use: <date> + or - <number><d, w, m, or y>";
            return false;
        }

        if (!TryParseDate(match.Groups["date"].Value.Trim(), culture, today, out var startDate))
        {
            error = $"'{match.Groups["date"].Value.Trim()}' is not a recognized date";
            return false;
        }

        if (!int.TryParse(match.Groups["amount"].Value, NumberStyles.None, CultureInfo.InvariantCulture, out var amount))
        {
            error = "The duration is too large";
            return false;
        }

        if (match.Groups["operator"].Value[0] == '-')
        {
            amount = -amount;
        }

        try
        {
            var result = char.ToLowerInvariant(match.Groups["unit"].Value[0]) switch
            {
                'd' => startDate.AddDays(amount),
                'w' => startDate.AddDays(checked(amount * 7)),
                'm' => startDate.AddMonths(amount),
                'y' => startDate.AddYears(amount),
                _ => throw new InvalidOperationException("Unsupported duration unit."),
            };

            calculation = new DateCalculation(startDate, result);
            error = string.Empty;
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            error = "The result is outside the supported date range";
            return false;
        }
        catch (OverflowException)
        {
            error = "The duration is too large";
            return false;
        }
    }

    private static bool TryParseDate(string value, CultureInfo culture, DateOnly today, out DateOnly date)
    {
        switch (value.ToLowerInvariant())
        {
            case "today":
                date = today;
                return true;
            case "tomorrow":
                return TryAddAliasDays(today, 1, out date);
            case "yesterday":
                return TryAddAliasDays(today, -1, out date);
        }

        if (DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            return true;
        }

        return DateOnly.TryParse(value, culture, DateTimeStyles.AllowWhiteSpaces, out date);
    }

    private static bool TryAddAliasDays(DateOnly today, int days, out DateOnly date)
    {
        try
        {
            date = today.AddDays(days);
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            date = default;
            return false;
        }
    }

    [GeneratedRegex(
        @"^\s*(?<date>.+?)\s*(?<operator>[+-])\s*(?<amount>\d+)\s*(?<unit>[dwmy])\s*$",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ExpressionPattern();
}
