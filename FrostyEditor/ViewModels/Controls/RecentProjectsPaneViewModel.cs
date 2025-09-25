using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using FrostyEditor.Services;
using FrostyEditor.ViewModels.Data;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Controls;

public partial class RecentProjectsPaneViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly IRecentProjectsService m_recentProjectsService;

    public ViewModelActivator Activator { get; } = new();

    [ObservableAsProperty]
    private IEnumerable<RecentProjectViewModel> m_recentProjects = [];

    public RecentProjectsPaneViewModel(IRecentProjectsService recentProjectsService)
    {
        m_recentProjectsService = recentProjectsService;

        m_recentProjectsHelper = LoadRecentProjectsCommand.ToProperty(this, nameof(RecentProjects), scheduler: RxApp.MainThreadScheduler);

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            LoadRecentProjectsCommand.Execute().Subscribe();
        });
    }

    [ReactiveCommand]
    private async Task<IEnumerable<RecentProjectViewModel>> LoadRecentProjects()
    {
        return (await m_recentProjectsService.GetRecentProjectsAsync()).Select(entry => new RecentProjectViewModel(entry)).ToImmutableList();
    }
}