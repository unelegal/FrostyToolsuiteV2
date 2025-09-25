using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Frosty.ModSupport.Project;
using FrostyEditor.Services;
using FrostyEditor.ViewModels.Controls;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Windows;

public partial class NewProjectWindowViewModel : ViewModelBase
{
    public required ProfilePickerViewModel ProfilePickerViewModel { get; init; }

    public required IProjectService ProjectService { private get; init; }

    public required IDialogService DialogService { private get; init; }

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
        string? path = ProjectService.CreateProject(ModName, ModVersion, ProjectPath, ProfilePickerViewModel.SelectedProfile!.Slug);

        await DialogService.CloseCurrentWindowWithData.Handle(path);
    }

    [ReactiveCommand]
    private async Task Cancel() => await DialogService.CloseCurrentWindow.Handle(Unit.Default);

    [ReactiveCommand]
    private async Task<string?> PickFolder()
    {
        var folders = await DialogService.OpenFolderPicker.Handle(new FolderPickerOpenOptions()
        {
            Title = "Choose a game",
            AllowMultiple = false
        });

        return  folders.Count > 0 ? folders[0].TryGetLocalPath() : null;
    }
}