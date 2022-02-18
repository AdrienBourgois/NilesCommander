using System;

namespace NilesCommander.Core;

public readonly struct Log
{
    public enum LogSeverity
    {
        Debug,
        Verbose,
        Log,
        Warning,
        Error
    }

    public enum LogSource
    {
        Module,
        Niles,
        Framework
    }

    public DateTime Time { get; }
    public LogSeverity Severity { get; }
    public LogSource Source { get; }
    public string Text { get; }

    public Log(LogSeverity _severity, LogSource _source, string _log)
    {
        Time = DateTime.Now;
        Severity = _severity;
        Source = _source;
        Text = _log;
    }

    public override string ToString()
    {
        return $"[{Time:G}] ({Severity}) - {Source} : {Text}";
    }
}