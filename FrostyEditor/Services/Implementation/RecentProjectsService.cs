using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using FrostyEditor.Models;
using Newtonsoft.Json;

namespace FrostyEditor.Services.Implementation;

public class RecentProjectsService : IRecentProjectsService
{
    private static readonly string s_recentProjectsFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FrostyEditorV2", "RecentProjects.json");

    private const int c_maxRecentProjects = 10;

    private readonly object m_recentProjectsLock = new();
    private readonly SourceCache<RecentProjectEntry, string> m_recentProjects = new(e => e.FullPath);

    private IEnumerable<RecentProjectEntry> LoadRecentProjects()
    {
        if (!File.Exists(s_recentProjectsFile))
        {
            return [];
        }

        lock (m_recentProjectsLock)
        {
            string content = File.ReadAllText(s_recentProjectsFile);
            var recentProjects = JsonConvert.DeserializeObject<IList<RecentProjectEntry>>(content) ?? [];
            m_recentProjects.EditDiff(recentProjects, EqualityComparer<RecentProjectEntry>.Default);

            return recentProjects;
        }
    }

    private void SaveRecentProjects(IEnumerable<RecentProjectEntry> recentProjects)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(s_recentProjectsFile)!);

        lock (m_recentProjectsLock)
        {
            File.WriteAllText(s_recentProjectsFile, JsonConvert.SerializeObject(recentProjects));

            m_recentProjects.EditDiff(recentProjects, EqualityComparer<RecentProjectEntry>.Default);
        }
    }

    public IObservable<IChangeSet<RecentProjectEntry, string>> ConnectRecentProjects() => m_recentProjects.Connect();

    public void RefreshRecentProjects()
    {
        LoadRecentProjects();
    }

    public void ProjectOpened(string path)
    {
        lock (m_recentProjectsLock)
        {
            List<RecentProjectEntry> entries = LoadRecentProjects().ToList();

            entries.RemoveAll(p => Path.GetFullPath(path) == Path.GetFullPath(p.FullPath));
            entries.Add(new RecentProjectEntry { FullPath = path, LastOpened = DateTime.Now });

            SaveRecentProjects(entries.OrderByDescending(p => p.LastOpened).Take(c_maxRecentProjects));
        }
    }
}