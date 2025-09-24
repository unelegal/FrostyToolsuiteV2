using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FrostyEditor.ViewModels.Controls;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Windows;

public partial class NewProjectWindowViewModel : ViewModelBase
{
    public required ProfilePickerViewModel ProfilePickerViewModel { get; init; }

    public Interaction<string?, Unit> CloseDialogInteraction { get; } = new();

    public Interaction<Unit, string?> PickFolderInteraction { get; } = new();

    [Reactive]
    private string m_modName = string.Empty;

    [Reactive]
    private string m_modVersion = string.Empty;

    [Reactive]
    private string m_projectPath = string.Empty;

    private readonly IObservable<bool> m_canCreateProject;

    public NewProjectWindowViewModel()
    {
        m_canCreateProject = this.WhenAnyValue(x => x.ModName, x => x.ModVersion, x => x.ProjectPath, x => x.ProfilePickerViewModel.SelectedProfile,
                (name, version, path, profile) => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(version) && !string.IsNullOrWhiteSpace(path) &&
                                                  !string.IsNullOrWhiteSpace(profile?.Slug))
            .SubscribeOn(RxApp.MainThreadScheduler);

        PickFolderCommand.SubscribeOn(RxApp.MainThreadScheduler).Subscribe(path =>
        {
            if (path is null)
            {
                return;
            }

            ProjectPath = path;
        });
    }

    [ReactiveCommand(CanExecute = nameof(m_canCreateProject))]
    private async Task Create()
    {
        await CloseDialogInteraction.Handle(null);
    }

    [ReactiveCommand]
    private async Task Cancel() => await CloseDialogInteraction.Handle(null);

    [ReactiveCommand]
    private async Task<string?> PickFolder() => await PickFolderInteraction.Handle(Unit.Default);
}