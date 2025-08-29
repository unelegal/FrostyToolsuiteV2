using System.Collections.ObjectModel;
using FrostyEditor.ViewModels.Controls;

namespace FrostyEditor.ViewModels.Windows;

public class ProjectWindowViewModel : ViewModelBase
{
    public RecentProjectsPaneViewModel RecentProjects { get; }

    public ProjectWindowViewModel(RecentProjectsPaneViewModel recentProjectsPaneViewModel)
    {
        RecentProjects = recentProjectsPaneViewModel;
    }


}