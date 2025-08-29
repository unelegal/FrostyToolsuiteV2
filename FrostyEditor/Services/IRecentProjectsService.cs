using System.Collections.Generic;
using System.ComponentModel;
using FrostyEditor.Models;

namespace FrostyEditor.Services;

public interface IRecentProjectsService : INotifyPropertyChanged
{
    public List<RecentProject> RecentProjects { get; }
}