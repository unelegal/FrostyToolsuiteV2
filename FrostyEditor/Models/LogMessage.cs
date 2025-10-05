using System;

namespace FrostyEditor.Models;

public enum LogLevel
{
    Trace,
    Debug,
    Info,
    Warn,
    Error,
    Fatal
}

public record LogMessage(DateTime Timestamp, LogLevel Level, string Message)
{
    public LogMessage(string message, LogLevel level = LogLevel.Info) : this(DateTime.Now, level, message) { }

    public override string ToString() => $"[{Timestamp:HH:mm:ss}] [{Level}] {Message}";
}