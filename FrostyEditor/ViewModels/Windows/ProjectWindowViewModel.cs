using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FrostyEditor.Services;
using FrostyEditor.ViewModels.Controls;
using Newtonsoft.Json.Converters;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Windows;

public partial class ProjectWindowViewModel : ViewModelBase
{
    public required IRecentProjectsService RecentProjectsService { private get; init; }

    public required RecentProjectsPaneViewModel RecentProjects { get; init; }

    public Interaction<Unit, string?> CreateProjectInteraction { get; } = new();
    public Interaction<Unit, string?> OpenProjectInteraction { get; } = new();
    public Interaction<Unit, Unit> OpenProfileManagerInteraction { get; } = new();
    public Interaction<Unit, Unit> OpenKeyManagerInteraction { get; } = new();

    [ReactiveCommand]
    private async Task CreateProject()
    {
        string? projectPath = await CreateProjectInteraction.Handle(Unit.Default);
        if (projectPath is null)
        {
            return;
        }

        await RecentProjectsService.ProjectOpened(projectPath);
        await RecentProjects.LoadRecentProjectsCommand.Execute();
    }

    [ReactiveCommand]
    private async Task OpenProject()
    {
        string? chosenPath = await OpenProjectInteraction.Handle(Unit.Default);
        if (chosenPath is null)
        {
            return;
        }

        await RecentProjectsService.ProjectOpened(chosenPath);
        await RecentProjects.LoadRecentProjectsCommand.Execute();
    }

    [ReactiveCommand]
    private async Task OpenProfileManager() => await OpenProfileManagerInteraction.Handle(Unit.Default);

    [ReactiveCommand]
    private async Task OpenKeyManager() => await OpenKeyManagerInteraction.Handle(Unit.Default);
}