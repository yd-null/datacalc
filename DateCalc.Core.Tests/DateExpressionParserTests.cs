using System.Globalization;
using DateCalc.Core;
using Xunit;

namespace DateCalc.Core.Tests;

public sealed class DateExpressionParserTests
{
    private static readonly DateOnly Today = new(2026, 8, 17);

    [Theory]
    [InlineData("today + 30d", "2026-09-16")]
    [InlineData("tomorrow - 1d", "2026-08-17")]
    [InlineData("yesterday + 2w", "2026-08-30")]
    [InlineData("2024-01-31 + 1m", "2024-02-29")]
    [InlineData("2024-02-29 + 1y", "2025-02-28")]
    [InlineData(" 2026-08-17+3D ", "2026-08-20")]
    public void CalculatesAliasesIsoDatesAndCalendarUnits(string expression, string expected)
    {
        var success = DateExpressionParser.TryCalculate(
            expression,
            CultureInfo.GetCultureInfo("en-US"),
            Today,
            out var calculation,
            out var error);

        Assert.True(success, error);
        Assert.Equal(DateOnly.ParseExact(expected, "yyyy-MM-dd", CultureInfo.InvariantCulture), calculation.ResultDate);
    }

    [Theory]
    [InlineData("en-US", "8/17/2026 + 1d")]
    [InlineData("en-GB", "17/08/2026 + 1d")]
    [InlineData("de-DE", "17.08.2026 + 1d")]
    public void ParsesDatesUsingCurrentCulture(string cultureName, string expression)
    {
        var success = DateExpressionParser.TryCalculate(
            expression,
            CultureInfo.GetCultureInfo(cultureName),
            Today,
            out var calculation,
            out var error);

        Assert.True(success, error);
        Assert.Equal(new DateOnly(2026, 8, 18), calculation.ResultDate);
    }

    [Theory]
    [InlineData("")]
    [InlineData("today")]
    [InlineData("today + d")]
    [InlineData("today * 2d")]
    [InlineData("not-a-date + 2d")]
    [InlineData("today + 999999999999d")]
    public void RejectsInvalidExpressions(string expression)
    {
        var success = DateExpressionParser.TryCalculate(
            expression,
            CultureInfo.GetCultureInfo("en-US"),
            Today,
            out _,
            out var error);

        Assert.False(success);
        Assert.NotEmpty(error);
    }
}
