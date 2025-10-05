using System;
using DynamicData;
using FrostyEditor.Models;
using FrostyEditor.Utilities;

namespace FrostyEditor.Services.Implementation;

public class LoggingService : ILoggingService
{
    private const uint c_defaultCapacity = 1000;

    private readonly SourceList<LogMessage> m_messageSource = new();

    public uint Capacity { get; }

    public LoggingService(uint capacity = c_defaultCapacity)
    {
        Capacity = Math.Max(1, capacity);
    }

    public void LogInfo(string message) => Log(new LogMessage(message, LogLevel.Info));

    public void LogWarning(string message) => Log(new LogMessage(message, LogLevel.Warn));

    public void LogError(string message) => Log(new LogMessage(message, LogLevel.Error));

    public void LogProgress(double progress)
    {
        // TODO
    }

    public IObservable<IChangeSet<LogMessage>> ConnectToMessages() => m_messageSource.Connect();

    public void ClearLog()
    {
        m_messageSource.Clear();
    }

    private void Log(LogMessage message)
    {
        Async.RunInBackground(async () =>
        {
            m_messageSource.Edit(updater =>
            {
                updater.Add(message);
                while (updater.Count > Capacity)
                {
                    updater.RemoveAt(0);
                }
            });
        }).ConfigureAwait(false);
    }

}