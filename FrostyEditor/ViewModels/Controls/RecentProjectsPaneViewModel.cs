using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using FrostyEditor.Services;
using ReactiveUI;

namespace FrostyEditor.ViewModels.Controls;

public class RecentProjectsPaneViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly IRecentProjectsService m_recentProjectsService;

    public ViewModelActivator Activator { get; }

    private readonly ObservableAsPropertyHelper<IEnumerable<RecentProjectViewModel>> m_recentProjects;
    public IEnumerable<RecentProjectViewModel> RecentProjects => m_recentProjects.Value;
    public ReactiveCommand<Unit, IEnumerable<RecentProjectViewModel>> LoadRecentProjects { get; }

    public RecentProjectsPaneViewModel(IRecentProjectsService recentProjectsService)
    {
        m_recentProjectsService = recentProjectsService;
        Activator = new ViewModelActivator();

        LoadRecentProjects = ReactiveCommand.CreateFromTask(LoadRecentProjectsAsync);
        m_recentProjects = LoadRecentProjects.ToProperty(this, nameof(RecentProjects), scheduler: RxApp.MainThreadScheduler);

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            LoadRecentProjects.Execute().Subscribe();
        });
    }

    private async Task<IEnumerable<RecentProjectViewModel>> LoadRecentProjectsAsync()
    {
        //await m_recentProjectsService.ProjectOpened("C:\\Users\\Test\\Project\\project.json");
        return (await m_recentProjectsService.GetRecentProjectsAsync()).Select(entry => new RecentProjectViewModel(entry)).ToImmutableList();
    }
}