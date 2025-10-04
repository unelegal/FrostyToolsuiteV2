using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FrostyEditor.ViewModels.Windows;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace FrostyEditor.Views.Windows;

public partial class ProcessSelectorWindow : ReactiveWindow<ProcessSelectorViewModel>
{
    public ProcessSelectorWindow()
    {
        this.WhenActivated(disposables =>
        {
            ((ListBox) ProcessList)
                .Events()
                .DoubleTapped
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel!.SelectProcessCommand)
                .DisposeWith(disposables);
        });
        InitializeComponent();
    }

    public ProcessSelectorWindow(ProcessSelectorViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}