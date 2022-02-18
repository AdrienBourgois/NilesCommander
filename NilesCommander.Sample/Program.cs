using System;
using System.Threading.Tasks;
using NilesCommander.Core;
using NilesCommander.Terminal;

namespace NilesCommander.Sample;

internal class Program
{
    private static bool looping = true;

    private static async Task Main(string[] _args)
    {
        NilesCommanderConfiguration niles_commander_configuration = new();

        Core.NilesCommander niles_commander = new(niles_commander_configuration);

        niles_commander.AddComponent<TerminalComponent, TerminalConfiguration>(new TerminalConfiguration());
        niles_commander.Initialize();

        niles_commander.CommandProvided += OnCommandProvided;

        while (looping)
        {
            await Task.Delay(2000);
            niles_commander.Log(Log.LogSeverity.Log, Log.LogSource.Niles, "Loop");
        }

        niles_commander.Dispose();
    }

    private static void OnCommandProvided(string _command)
    {
        if (_command == "stop")
            looping = false;
    }
}