using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FrostyEditor.Services;
using FrostyEditor.ViewModels.Controls;
using ReactiveUI;

namespace FrostyEditor.ViewModels.Windows;

public class ProjectWindowViewModel : ViewModelBase
{
    private readonly IRecentProjectsService m_recentProjectsService;

    public RecentProjectsPaneViewModel RecentProjects { get; }

    public Interaction<Unit, string?> CreateProjectInteraction { get; }
    public Interaction<Unit, string?> OpenProjectInteraction { get; }

    public ReactiveCommand<Unit, Unit> CreateProjectCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenProjectCommand { get; }

    public ProjectWindowViewModel(RecentProjectsPaneViewModel recentProjectsPaneViewModel, IRecentProjectsService recentProjectsService)
    {
        m_recentProjectsService = recentProjectsService;

        RecentProjects = recentProjectsPaneViewModel;
        CreateProjectInteraction =  new Interaction<Unit, string?>();
        OpenProjectInteraction = new Interaction<Unit, string?>();
        CreateProjectCommand = ReactiveCommand.CreateFromTask(CreateProjectAsync);
        OpenProjectCommand = ReactiveCommand.CreateFromTask(OpenProjectAsync);
    }

    private async Task CreateProjectAsync()
    {
        string? projectPath = await CreateProjectInteraction.Handle(Unit.Default);
        if (projectPath is null)
        {
            return;
        }

        await m_recentProjectsService.ProjectOpened(projectPath);
        await RecentProjects.LoadRecentProjects.Execute();
    }

    private async Task OpenProjectAsync()
    {
        string? chosenPath = await OpenProjectInteraction.Handle(Unit.Default);
        if (chosenPath is null)
        {
            return;
        }

        await m_recentProjectsService.ProjectOpened(chosenPath);
        await RecentProjects.LoadRecentProjects.Execute();
    }

}