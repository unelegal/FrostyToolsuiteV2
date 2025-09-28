using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicData;
using FrostyEditor.Models;

namespace FrostyEditor.Services.Implementation.Mock;

public class DesignRecentProjectsService : IRecentProjectsService
{
    private readonly SourceCache<RecentProjectEntry, string> m_recentProjects = new(e => e.FullPath);

    public DesignRecentProjectsService()
    {
        m_recentProjects.AddOrUpdate([
            new RecentProjectEntry { FullPath = "C:\\Users\\Frosty\\Projects\\Test1\\TestProject1.project", LastOpened = DateTime.Now - TimeSpan.FromDays(1) },
            new RecentProjectEntry { FullPath = "C:\\Users\\Frosty\\Projects\\Test2\\TestProject2.project", LastOpened = DateTime.Now - TimeSpan.FromDays(2) },
            new RecentProjectEntry { FullPath = "C:\\Users\\Frosty\\Projects\\Test3\\TestProject3.project", LastOpened = DateTime.Now - TimeSpan.FromDays(3) },
            new RecentProjectEntry { FullPath = "C:\\Users\\Frosty\\Projects\\Test4\\TestProject4.project", LastOpened = DateTime.Now - TimeSpan.FromDays(4) },
            new RecentProjectEntry { FullPath = "C:\\Users\\Frosty\\Projects\\Test5\\TestProject5.project", LastOpened = DateTime.Now - TimeSpan.FromDays(5) },
            new RecentProjectEntry { FullPath = "/home/frosty/projects/test6/TestProject6.project", LastOpened = DateTime.Now - TimeSpan.FromDays(6) },
            new RecentProjectEntry { FullPath = "/home/frosty/projects/test7/TestProject7.project", LastOpened = DateTime.Now - TimeSpan.FromDays(7) },
            new RecentProjectEntry { FullPath = "/home/frosty/projects/test8/TestProject8.project", LastOpened = DateTime.Now - TimeSpan.FromDays(8) },
            new RecentProjectEntry { FullPath = "/root.project", LastOpened = DateTime.Now - TimeSpan.FromDays(9) },
            new RecentProjectEntry { FullPath = "C:\\Root.project", LastOpened = DateTime.Now - TimeSpan.FromDays(10) }
        ]);
    }

    public IObservable<IChangeSet<RecentProjectEntry, string>> ConnectRecentProjects() => m_recentProjects.Connect();

    public async Task RefreshRecentProjects()
    {

    }

    public async Task ProjectOpened(string path)
    {
        m_recentProjects.AddOrUpdate(new RecentProjectEntry { FullPath = path, LastOpened = DateTime.Now });
    }
}