using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Dock.Model.ReactiveUI.Controls;
using DynamicData;
using FrostyEditor.Models;
using FrostyEditor.Services;
using FrostyEditor.Utilities;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Tools;

public partial class LogViewModel : Tool, IDisposable
{
    private readonly ILoggingService m_loggingService;
    private readonly CompositeDisposable m_disposables = new();

    private readonly ReadOnlyObservableCollection<LogMessage> m_logMessages;
    public ReadOnlyObservableCollection<LogMessage> LogMessages => m_logMessages;

    [Reactive]
    private LogLevel m_minimumLogLevel = LogLevel.Debug;

    [Reactive]
    private bool m_autoScroll = true;

    public IEnumerable<LogLevel> AvailableLogLevels { get; } = Enum.GetValues<LogLevel>();

    public LogViewModel(ILoggingService loggingService)
    {
        m_loggingService = loggingService;

        Title = "Logs";
        Id = "LogViewer";

        var filter = this
            .WhenAnyValue(x => x.MinimumLogLevel)
            .Select<LogLevel, Func<LogMessage, bool>>(minLevel => message => message.Level >= minLevel);

        m_loggingService.ConnectToMessages()
            .Filter(filter)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Sort(Comparer<LogMessage>.Create((a, b) =>
            {
                if (a.Timestamp == b.Timestamp)
                {
                    return 0;
                }

                if (a.Timestamp < b.Timestamp)
                {
                    return -1;
                }

                return 1;
            }))
            .Bind(out m_logMessages)
            .Subscribe()
            .DisposeWith(m_disposables);
    }

    [ReactiveCommand]
    private async Task ClearLog()
    {
        await Async.RunInBackground(async () => m_loggingService.ClearLog());
    }

    [ReactiveCommand]
    private void AddDummyMessage()
    {
        m_loggingService.LogWarning("Dummy message");
    }

    public void Dispose() => m_disposables.Dispose();
}