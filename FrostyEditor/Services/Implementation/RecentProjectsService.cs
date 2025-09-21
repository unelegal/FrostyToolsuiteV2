using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FrostyEditor.Models;
using Newtonsoft.Json;

namespace FrostyEditor.Services.Implementation;

public class RecentProjectsService : IRecentProjectsService
{
    private static readonly string s_recentProjectsFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FrostyEditorV2", "RecentProjects.json");

    private const int c_maxRecentProjects = 10;

    public async Task<IEnumerable<RecentProjectEntry>> GetRecentProjectsAsync()
    {
        if (!File.Exists(s_recentProjectsFile))
        {
            return [];
        }

        string content = await File.ReadAllTextAsync(s_recentProjectsFile);
        return (JsonConvert.DeserializeObject<IEnumerable<RecentProjectEntry>>(content) ?? []).OrderByDescending(e => e.LastOpened);
    }

    private async Task SaveRecentProjectsAsync(IEnumerable<RecentProjectEntry> projects)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(s_recentProjectsFile)!);

        await File.WriteAllTextAsync(s_recentProjectsFile, JsonConvert.SerializeObject(projects));
    }

    public async Task ProjectOpened(string path)
    {
        List<RecentProjectEntry> entries = (await GetRecentProjectsAsync()).ToList();

        entries.RemoveAll(p => Path.GetFullPath(path) == Path.GetFullPath(p.FullPath));
        entries.Add(new RecentProjectEntry { FullPath = path, LastOpened = DateTime.Now });

        await SaveRecentProjectsAsync(entries.OrderByDescending(p => p.LastOpened).Take(c_maxRecentProjects));
    }
}