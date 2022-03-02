using Konsole;
using NilesCommander.Core;
using NilesCommander.Core.Components;

namespace NilesCommander.Terminal;

public class TerminalComponent : IInputComponent, IOutputComponent
{
    public event Core.NilesCommander.CommandProvidedDelegate CommandProvided;

    private CancellationTokenSource cancellationTokenSource = new();

    public ComponentConfiguration Configuration { get; set; }

    private IConsole logsWindow;
    private IConsole commandWindow;

    private string CommandBuffer { get; set; } = string.Empty;

    public void SetComponentConfiguration(ComponentConfiguration _configuration)
    {
        Configuration = _configuration;
    }

    public bool Initialize()
    {
        Console.CursorVisible = false;
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        logsWindow = Window.OpenBox("Logs", Console.WindowWidth, Console.WindowHeight - 3);
        commandWindow = Window.OpenBox("Commands", Console.WindowWidth, 3);

        UpdateCommandBufferDisplay();

        Task.Run(ConsoleLoop, cancellationTokenSource.Token);

        return true;
    }

    void IOutputComponent.LogProvided(Log _log)
    {
        WriteLog(_log);
    }

    public bool Close()
    {
        cancellationTokenSource.Dispose();

        return true;
    }

    private void ConsoleLoop()
    {
        while (!cancellationTokenSource.IsCancellationRequested)
            ProcessKey(Console.ReadKey(true));
    }

    private void ProcessKey(ConsoleKeyInfo _key)
    {
        switch (_key.Key)
        {
            case ConsoleKey.Enter:
                if (string.IsNullOrEmpty(CommandBuffer))
                    break;
                CommandProvided?.Invoke(CommandBuffer);
                CommandBuffer = string.Empty;
                break;
            case ConsoleKey.Backspace:
                if (CommandBuffer.Length > 0)
                    CommandBuffer = CommandBuffer.Remove(CommandBuffer.Length - 1);
                break;
            case ConsoleKey.Escape:
                CommandBuffer = string.Empty;
                break;
            default:
                if (_key.KeyChar != '\0' && _key.KeyChar != '\t')
                    CommandBuffer += _key.KeyChar;
                break;
        }

        UpdateCommandBufferDisplay();
    }

    private void UpdateCommandBufferDisplay()
    {
        commandWindow.Clear(ConsoleColor.DarkCyan);
        commandWindow.Write("> " + CommandBuffer);
        Console.SetWindowPosition(0, 0);
    }

    public void WriteLog(Log _log)
    {
        ConsoleColor log_color = _log.Severity switch
        {
            Log.LogSeverity.Debug => ConsoleColor.DarkGray,
            Log.LogSeverity.Verbose => ConsoleColor.Gray,
            Log.LogSeverity.Log => ConsoleColor.White,
            Log.LogSeverity.Warning => ConsoleColor.Yellow,
            Log.LogSeverity.Error => ConsoleColor.Red,
            _ => ConsoleColor.White
        };

        logsWindow.ForegroundColor = log_color;
        logsWindow.WriteLine(_log.ToString());
        Console.SetWindowPosition(0, 0);
    }
}