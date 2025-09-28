using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using FrostyEditor.Services;
using FrostyEditor.ViewModels.Controls;
using Newtonsoft.Json.Converters;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Windows;

public partial class ProjectWindowViewModel : ViewModelBase
{
    public required IRecentProjectsService RecentProjectsService { private get; init; }
    public required IProjectService ProjectService { private get; init; }
    public required IDialogService DialogService { private get; init; }

    public required RecentProjectsPaneViewModel RecentProjects { get; init; }

    [ReactiveCommand]
    //private IObservable<Unit> CreateProject() => Observable.FromAsync(CreateProjectAsync).SubscribeOn(RxApp.TaskpoolScheduler);
    private async Task CreateProjectAsync()
    {
        string? projectPath = await DialogService.OpenCreateProject.Handle(Unit.Default);
        if (projectPath is null)
        {
            return;
        }

        await ProjectService.OpenProjectCommand.Execute(projectPath);
        RecentProjectsService.ProjectOpened(projectPath);
    }

    [ReactiveCommand]
    private async Task OpenProject()
    {

        var files = await DialogService.OpenFilePicker.Handle(new FilePickerOpenOptions
        {
            Title = Assets.Lang.Resources.OpenProject,
            AllowMultiple = false,
            FileTypeFilter =
            [
                new(Assets.Lang.Resources.FrostyProject) { Patterns = ["*.json"] }
            ]
        });

        string? chosenPath = files.Count > 0 ? files[0].TryGetLocalPath() : null;

        if (chosenPath is null)
        {
            return;
        }

        await ProjectService.OpenProjectCommand.Execute(chosenPath);
        RecentProjectsService.ProjectOpened(chosenPath);
    }

    [ReactiveCommand]
    private async Task OpenProfileManager() => await DialogService.OpenProfileManager.Handle(Unit.Default);

    // TODO
    [ReactiveCommand]
    private async Task OpenKeyManager() => await DialogService.OpenProfileManager.Handle(Unit.Default);
}