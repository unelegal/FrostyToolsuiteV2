using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using FrostyEditor.Models;
using FrostyEditor.Utilities;
using Newtonsoft.Json;

namespace FrostyEditor.Services.Implementation;

public class RecentProjectsService : IRecentProjectsService
{
    private static readonly string s_recentProjectsFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FrostyEditorV2", "RecentProjects.json");

    private const int c_maxRecentProjects = 10;

    private readonly AsyncSemaphore m_fileSemaphore = new(1, 1);
    private readonly SourceCache<RecentProjectEntry, string> m_recentProjects = new(e => e.FullPath);

    /// <summary>
    /// Load recent projects from disk. Only call with m_fileSemaphore acquired!
    /// </summary>
    /// <returns>List of recently opened projects</returns>
    private async Task<IEnumerable<RecentProjectEntry>> LoadRecentProjects()
    {
        if (!File.Exists(s_recentProjectsFile))
        {
            return [];
        }

        string content = await File.ReadAllTextAsync(s_recentProjectsFile);
        var recentProjects = JsonConvert.DeserializeObject<IList<RecentProjectEntry>>(content) ?? [];
        m_recentProjects.EditDiff(recentProjects, EqualityComparer<RecentProjectEntry>.Default);

        return recentProjects;
    }

    /// <summary>
    /// Save recent projects to disk. Only call with m_fileSemaphore acquired!
    /// </summary>
    /// <param name="recentProjects">The list of projects to save</param>
    private async Task SaveRecentProjects(IEnumerable<RecentProjectEntry> recentProjects)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(s_recentProjectsFile)!);

        await File.WriteAllTextAsync(s_recentProjectsFile, JsonConvert.SerializeObject(recentProjects));

        m_recentProjects.EditDiff(recentProjects, EqualityComparer<RecentProjectEntry>.Default);
    }

    public IObservable<IChangeSet<RecentProjectEntry, string>> ConnectRecentProjects() => m_recentProjects.Connect();

    public async Task RefreshRecentProjects()
    {
        using var _ = await m_fileSemaphore.WaitAsync();
        await LoadRecentProjects();
    }

    public async Task ProjectOpened(string path)
    {
        using var _ = await m_fileSemaphore.WaitAsync();

        List<RecentProjectEntry> entries = (await LoadRecentProjects()).ToList();

        entries.RemoveAll(p => Path.GetFullPath(path) == Path.GetFullPath(p.FullPath));
        entries.Add(new RecentProjectEntry { FullPath = path, LastOpened = DateTime.Now });

        await SaveRecentProjects(entries.OrderByDescending(p => p.LastOpened).Take(c_maxRecentProjects));
    }
}