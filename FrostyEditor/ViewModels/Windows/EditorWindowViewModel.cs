using System.Reactive.Disposables;
using Dock.Model.Controls;
using Dock.Model.ReactiveUI.Controls;
using FrostyEditor.Services;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Windows;

public partial class EditorWindowViewModel : ViewModelBase, IActivatableViewModel
{
    public required IDockingService DockingService { private get; init; }

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private IRootDock? m_layout;

    public EditorWindowViewModel()
    {
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            Layout = DockingService!.CreateDockingRoot();
        });
    }
}