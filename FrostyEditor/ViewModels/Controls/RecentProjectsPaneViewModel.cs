using System.Collections.Generic;
using System.Collections.ObjectModel;
using FrostyEditor.Models;
using FrostyEditor.Services;
using RecentProjectsService = FrostyEditor.Services.Implementation.RecentProjectsService;

namespace FrostyEditor.ViewModels.Controls;

public class RecentProjectsPaneViewModel : ViewModelBase
{
    private readonly IRecentProjectsService m_recentProjectsService;

    public RecentProjectsPaneViewModel(IRecentProjectsService recentProjectsService)
    {
        m_recentProjectsService = recentProjectsService;

        // Forward RecentProjects changes
        m_recentProjectsService.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(m_recentProjectsService.RecentProjects))
            {
                OnPropertyChanged(nameof(RecentProjects));
            }
        };
    }

    public List<RecentProject> RecentProjects => m_recentProjectsService.RecentProjects;
}