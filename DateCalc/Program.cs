using Microsoft.CommandPalette.Extensions;
using Shmuelie.WinRTServer;
using Shmuelie.WinRTServer.CsWinRT;

namespace DateCalc;

public static class Program
{
    [MTAThread]
    public static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] != "-RegisterProcessAsComServer")
        {
            return;
        }

        using ManualResetEvent extensionDisposedEvent = new(false);
        global::Shmuelie.WinRTServer.ComServer server = new();
        DateCalcExtension extension = new(extensionDisposedEvent);

        server.RegisterClass<DateCalcExtension, IExtension>(() => extension);
        server.Start();
        extensionDisposedEvent.WaitOne();
        server.Stop();
        server.UnsafeDispose();
    }
}
