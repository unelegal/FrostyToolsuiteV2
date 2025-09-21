using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using FrostyEditor.Models;

namespace FrostyEditor.Services;

public interface IRecentProjectsService
{
    /// <summary>
    /// Get the list of recent projects asynchronously
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<RecentProjectEntry>> GetRecentProjectsAsync();

    /// <summary>
    /// Notify the service that a project has been opened.
    /// Saves updated recent projects list to disk.
    /// </summary>
    /// <param name="path">Path of the project that has been opened</param>
    /// <returns></returns>
    public Task ProjectOpened(string path);
}