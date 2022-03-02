using NilesCommander.Core;
using NilesCommander.Terminal;
using NilesCommander.WinForm;

namespace NilesCommander.Sample;

internal class Program
{
    private static TaskCompletionSource taskCompletionSource;

    private static WinFormComponent winFormComponent;
    private static TerminalComponent terminalComponent;

    private static async Task Main(string[] _args)
    {
        NilesCommanderConfiguration niles_commander_configuration = new();

        Core.NilesCommander niles_commander = new(niles_commander_configuration);

        winFormComponent = niles_commander.CreateComponent<WinFormComponent, WinFormConfiguration>(new WinFormConfiguration());
        terminalComponent = niles_commander.CreateComponent<TerminalComponent, TerminalConfiguration>(new TerminalConfiguration());
        niles_commander.Initialize();

        niles_commander.CommandProvided += OnCommandProvided;

        taskCompletionSource = new TaskCompletionSource();

        await taskCompletionSource.Task.ConfigureAwait(false);

        niles_commander.Close();
    }

    private static void OnCommandProvided(string _command)
    {
        switch (_command)
        {
            case "stop":
                taskCompletionSource.SetResult();
                break;
            case "openWindow":
                winFormComponent.RunForm();
                break;
        }
    }
}