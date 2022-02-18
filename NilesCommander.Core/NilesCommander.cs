using System;
using System.Collections.Generic;
using NilesCommander.Core.Components;

namespace NilesCommander.Core;

public class NilesCommander : IDisposable
{
    public delegate void CommandProvidedDelegate(string _command);

    public delegate void LogProvidedDelegate(Log _log);

    public NilesCommanderConfiguration NilesCommanderConfiguration { get; init; }

    public event CommandProvidedDelegate CommandProvided;

    public event LogProvidedDelegate LogProvided;

    private List<IInputComponent> inputs = new();

    private List<IOutputComponent> outputs = new();

    public NilesCommander(NilesCommanderConfiguration _nilesCommanderConfiguration)
    {
        NilesCommanderConfiguration = _nilesCommanderConfiguration;
    }

    public void AddComponent<T, ComponentConfig>(ComponentConfig _config) where T : IComponent, new() where ComponentConfig : ComponentConfiguration
    {
        T component = new();
        component.SetComponentConfiguration(_config);

        if (component is IInputComponent input_component)
        {
            inputs.Add(input_component);
            input_component.CommandProvided += OnCommandProvided;
        }

        if (component is IOutputComponent output_component)
            outputs.Add(output_component);
    }

    private void OnCommandProvided(string _command)
    {
        Log(Core.Log.LogSeverity.Debug, Core.Log.LogSource.Niles, $"Command provided : {_command}");

        CommandProvided?.Invoke(_command);
    }

    public void Initialize()
    {
        foreach (IInputComponent input_component in inputs)
            input_component.Initialize();

        foreach (IOutputComponent output_component in outputs)
            output_component.Initialize();
    }

    public void Log(Log.LogSeverity _severity, Log.LogSource _source, string _log, bool _sendToServer = false)
    {
        Log log = new(_severity, _source, _log);

        Log(log);
    }

    public void Log(Log _log)
    {
        foreach (IOutputComponent output_component in outputs)
            output_component.LogProvided(_log);

        LogProvided?.Invoke(_log);
    }

    public void Dispose()
    {
        foreach (IInputComponent input_component in inputs)
            input_component.Dispose();

        foreach (IOutputComponent output_component in outputs)
            output_component.Dispose();
    }
}