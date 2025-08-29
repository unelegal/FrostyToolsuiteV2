using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FrostyEditor.Models;
using Newtonsoft.Json;

namespace FrostyEditor.Services.Implementation;

public partial class RecentProjectsService : ObservableObject, IRecentProjectsService
{
    // TODO: Custom serializer for RecentProject instead of this
    private class ProjectEntry(string path, DateTime lastOpened)
    {
        public string Path { get; set; } = path;
        public DateTime LastOpened { get; set; } = lastOpened;
    }

    private static readonly string s_recentProjectsFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FrostyEditorV2", "RecentProjects.json");

    private const int c_maxRecentProjects = 10;

    [ObservableProperty]
    private List<RecentProject> m_recentProjects = [];

    public async Task LoadRecentProjectsAsync()
    {
        if (!File.Exists(s_recentProjectsFile))
        {
            return;
        }

        await using FileStream stream = File.OpenRead(s_recentProjectsFile);
        ProjectEntry[]? entries = JsonConvert.DeserializeObject<ProjectEntry[]>(await new StreamReader(stream).ReadToEndAsync());

        if (entries is not null)
        {
            RecentProjects = entries.OrderByDescending(p => p.LastOpened).Select(p => new RecentProject(p.Path, p.LastOpened)).ToList();
        }
    }

    private async Task SaveRecentProjectsAsync()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(s_recentProjectsFile)!);

        await using FileStream stream = File.Create(s_recentProjectsFile);
        await new StreamWriter(stream).WriteAsync(JsonConvert.SerializeObject(RecentProjects.Select(p => new ProjectEntry(p.FullPath, p.LastOpened)).ToArray()));
    }

    public async Task ProjectOpened(string path)
    {
        RecentProjects.RemoveAll(p => Path.GetFullPath(path) == Path.GetFullPath(p.FullPath));
        RecentProjects.Add(new RecentProject(path, DateTime.Now));
        RecentProjects = RecentProjects.OrderByDescending(p => p.LastOpened).Take(c_maxRecentProjects).ToList();
        await SaveRecentProjectsAsync();
    }
}