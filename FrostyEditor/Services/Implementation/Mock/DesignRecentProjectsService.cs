using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using FrostyEditor.Models;

namespace FrostyEditor.Services.Implementation.Mock;

public class DesignRecentProjectsService : ObservableObject, IRecentProjectsService
{
    public List<RecentProject> RecentProjects { get; } = [];

    public DesignRecentProjectsService()
    {
        RecentProjects.Add(new RecentProject("C:\\Users\\Frosty\\Projects\\Test1\\TestProject1.project", DateTime.Now - TimeSpan.FromDays(1)));
        RecentProjects.Add(new RecentProject("C:\\Users\\Frosty\\Projects\\Test2\\TestProject2.project", DateTime.Now - TimeSpan.FromDays(2)));
        RecentProjects.Add(new RecentProject("C:\\Users\\Frosty\\Projects\\Test3\\TestProject3.project", DateTime.Now - TimeSpan.FromDays(3)));
        RecentProjects.Add(new RecentProject("C:\\Users\\Frosty\\Projects\\Test4\\TestProject4.project", DateTime.Now - TimeSpan.FromDays(4)));
        RecentProjects.Add(new RecentProject("C:\\Users\\Frosty\\Projects\\Test5\\TestProject5.project", DateTime.Now - TimeSpan.FromDays(5)));
        RecentProjects.Add(new RecentProject("/home/frosty/projects/test6/TestProject6.project", DateTime.Now - TimeSpan.FromDays(6)));
        RecentProjects.Add(new RecentProject("/home/frosty/projects/test7/TestProject7.project", DateTime.Now - TimeSpan.FromDays(7)));
        RecentProjects.Add(new RecentProject("/home/frosty/projects/test8/TestProject8.project", DateTime.Now - TimeSpan.FromDays(8)));
        RecentProjects.Add(new RecentProject("/root.project", DateTime.Now - TimeSpan.FromDays(9)));
        RecentProjects.Add(new RecentProject("C:\\Root.project", DateTime.Now - TimeSpan.FromDays(10)));
    }
}