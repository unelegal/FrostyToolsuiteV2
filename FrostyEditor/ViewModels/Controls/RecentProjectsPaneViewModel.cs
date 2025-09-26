using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using FrostyEditor.Services;
using FrostyEditor.ViewModels.Data;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Controls;

public partial class RecentProjectsPaneViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly IRecentProjectsService m_recentProjectsService;

    public ViewModelActivator Activator { get; } = new();

    private readonly ReadOnlyObservableCollection<RecentProjectViewModel> m_recentProjects;
    public ReadOnlyObservableCollection<RecentProjectViewModel> RecentProjects => m_recentProjects;

    public RecentProjectsPaneViewModel(IRecentProjectsService recentProjectsService)
    {
        m_recentProjectsService = recentProjectsService;

        m_recentProjectsService.ConnectRecentProjects()
            .SortBy(x => x.LastOpened, SortDirection.Descending)
            .Transform(e => new RecentProjectViewModel(e))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out m_recentProjects)
            .Subscribe();

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            RefreshRecentProjectsCommand.Execute().Subscribe();
        });
    }

    [ReactiveCommand]
    private async Task RefreshRecentProjects()
    {
        m_recentProjectsService.RefreshRecentProjects();
    }
}