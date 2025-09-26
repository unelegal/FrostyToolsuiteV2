using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DynamicData;
using FrostyEditor.Models;

namespace FrostyEditor.Services;

public interface IRecentProjectsService
{
    /// <summary>
    /// Get an observable changeset of recent projects
    /// </summary>
    /// <returns></returns>
    public IObservable<IChangeSet<RecentProjectEntry, string>> ConnectRecentProjects();

    /// <summary>
    /// Refresh the recently opened projects from disk
    /// </summary>
    public void RefreshRecentProjects();

    /// <summary>
    /// Notify the service that a project has been opened.
    /// Saves updated recent projects list to disk.
    /// </summary>
    /// <param name="path">Path of the project that has been opened</param>
    public void ProjectOpened(string path);
}