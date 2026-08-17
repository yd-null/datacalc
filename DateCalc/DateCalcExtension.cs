using System.Runtime.InteropServices;
using Microsoft.CommandPalette.Extensions;

namespace DateCalc;

[Guid("432EB8D3-949A-4F4D-AAF5-893ED3AEDB8A")]
public sealed partial class DateCalcExtension : IExtension, IDisposable
{
    private readonly ManualResetEvent _extensionDisposedEvent;
    private readonly DateCalcCommandsProvider _provider = new();

    public DateCalcExtension(ManualResetEvent extensionDisposedEvent)
    {
        _extensionDisposedEvent = extensionDisposedEvent;
    }

    public object? GetProvider(ProviderType providerType) => providerType switch
    {
        ProviderType.Commands => _provider,
        _ => null,
    };

    public void Dispose()
    {
        _provider.Dispose();
        _extensionDisposedEvent.Set();
    }
}
