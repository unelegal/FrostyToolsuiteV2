using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FrostyEditor.Models;

namespace FrostyEditor.Services.Implementation.Mock;

public class DesignRecentProjectsService : IRecentProjectsService
{
    private readonly List<RecentProjectEntry> m_recentProjects = [];

    public DesignRecentProjectsService()
    {
        m_recentProjects.Add(
            new RecentProjectEntry { FullPath = "C:\\Users\\Frosty\\Projects\\Test1\\TestProject1.project", LastOpened = DateTime.Now - TimeSpan.FromDays(1) });
        m_recentProjects.Add(
            new RecentProjectEntry { FullPath = "C:\\Users\\Frosty\\Projects\\Test2\\TestProject2.project", LastOpened = DateTime.Now - TimeSpan.FromDays(2) });
        m_recentProjects.Add(
            new RecentProjectEntry { FullPath = "C:\\Users\\Frosty\\Projects\\Test3\\TestProject3.project", LastOpened = DateTime.Now - TimeSpan.FromDays(3) });
        m_recentProjects.Add(
            new RecentProjectEntry { FullPath = "C:\\Users\\Frosty\\Projects\\Test4\\TestProject4.project", LastOpened = DateTime.Now - TimeSpan.FromDays(4) });
        m_recentProjects.Add(
            new RecentProjectEntry { FullPath = "C:\\Users\\Frosty\\Projects\\Test5\\TestProject5.project", LastOpened = DateTime.Now - TimeSpan.FromDays(5) });
        m_recentProjects.Add(new RecentProjectEntry { FullPath = "/home/frosty/projects/test6/TestProject6.project", LastOpened = DateTime.Now - TimeSpan.FromDays(6) });
        m_recentProjects.Add(new RecentProjectEntry { FullPath = "/home/frosty/projects/test7/TestProject7.project", LastOpened = DateTime.Now - TimeSpan.FromDays(7) });
        m_recentProjects.Add(new RecentProjectEntry { FullPath = "/home/frosty/projects/test8/TestProject8.project", LastOpened = DateTime.Now - TimeSpan.FromDays(8) });
        m_recentProjects.Add(new RecentProjectEntry { FullPath = "/root.project", LastOpened = DateTime.Now - TimeSpan.FromDays(9) });
        m_recentProjects.Add(new RecentProjectEntry { FullPath = "C:\\Root.project", LastOpened = DateTime.Now - TimeSpan.FromDays(10) });
    }


    public async Task<IEnumerable<RecentProjectEntry>> GetRecentProjectsAsync()
    {
        return m_recentProjects;
    }

    public async Task ProjectOpened(string path)
    {
    }
}