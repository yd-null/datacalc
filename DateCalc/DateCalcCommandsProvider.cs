using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace DateCalc;

public sealed partial class DateCalcCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public DateCalcCommandsProvider()
    {
        Id = "io.github.yd-null.datacalc";
        DisplayName = "Date Calculator";
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        _commands =
        [
            new CommandItem(new DateCalculatorPage())
            {
                Title = DisplayName,
                Subtitle = "Add or subtract days, weeks, months, and years",
            },
        ];
    }

    public override ICommandItem[] TopLevelCommands() => _commands;
}
