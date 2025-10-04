using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using FrostyEditor.ViewModels.Data;
using ReactiveUI;

namespace FrostyEditor.Services.Implementation;

public class ProcessService : IProcessService, IDisposable
{
    private readonly SourceCache<Process, int> m_processes = new(x => x.Id);
    private readonly CompositeDisposable m_disposables = new();

    public ProcessService()
    {
        Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(3), scheduler: RxApp.TaskpoolScheduler)
            .Select(_ => Process.GetProcesses())
            .SubscribeOn(RxApp.TaskpoolScheduler)
            .Subscribe(processes =>
            {
                m_processes.EditDiff(processes, EqualityComparer<Process>.Default);
            })
            .DisposeWith(m_disposables);
    }

    public IObservable<IChangeSet<Process, int>> ConnectProcesses() => m_processes.Connect();

    public void Dispose() => m_disposables.Dispose();
}