using System;
using FrostyEditor.Models;

namespace FrostyEditor.ViewModels.Data;

public class RecentProjectViewModel : ViewModelBase
{
    public string Name => m_entry.Name;
    public string FullPath => m_entry.FullPath;
    public DateTime LastOpened => m_entry.LastOpened;

    private readonly RecentProjectEntry m_entry;

    public RecentProjectViewModel(RecentProjectEntry entry)
    {
        m_entry = entry;
    }

}