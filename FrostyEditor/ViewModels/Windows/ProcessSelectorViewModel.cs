using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using FrostyEditor.Services;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels.Data;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Windows;

public partial class ProcessSelectorViewModel : ViewModelBase, IActivatableViewModel
{
    private IProcessService m_processService;
    public required IDialogService DialogService { private get; init; }
    public ViewModelActivator Activator { get; } = new();

    private ReadOnlyObservableCollection<ProcessViewModel> m_processes;
    public ReadOnlyObservableCollection<ProcessViewModel> Processes => m_processes;

    [Reactive]
    private ProcessViewModel? m_selectedProcess;

    private IObservable<bool> m_isProcessSelected;

    public ProcessSelectorViewModel(IProcessService processService)
    {
        m_processService = processService;
        m_processService.ConnectProcesses()
            .Transform(p => new ProcessViewModel(p))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out m_processes)
            .Subscribe();

        m_isProcessSelected = this.WhenAnyValue(x => x.SelectedProcess)
            .Select(selected => selected is not null);
    }

    [ReactiveCommand(CanExecute = nameof(m_isProcessSelected))]
    private void SelectProcess()
    {
        DialogService.CloseCurrentWindow(SelectedProcess!.Pid);
    }

    [ReactiveCommand]
    private void CloseWindow()
    {
        DialogService.CloseCurrentWindow();
    }
}