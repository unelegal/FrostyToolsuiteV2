using System;
using DynamicData;
using Frosty.Sdk.Interfaces;
using FrostyEditor.Models;

namespace FrostyEditor.Services;

public interface ILoggingService : ILogger
{
    IObservable<IChangeSet<LogMessage>> ConnectToMessages();

    public void ClearLog();
}