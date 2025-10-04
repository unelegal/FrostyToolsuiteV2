using System;
using System.Reactive;
using System.Threading.Tasks;
using DynamicData;
using Frosty.ModSupport.Project;
using Frosty.Sdk.Managers.Entries;
using ReactiveUI;

namespace FrostyEditor.Services;

public interface IProjectService
{

    /// <summary>
    /// Create a project in a new folder
    /// </summary>
    /// <param name="modName">The Mod name</param>
    /// <param name="modVersion">The Mod version</param>
    /// <param name="createInFolder">The parent folder. A new subfolder will be created for the project</param>
    /// <param name="profileSlug">The profile instance slug to associate with the project</param>
    /// <returns>The project path if successful</returns>
    public Task<string?> CreateProject(string modName, string modVersion, string createInFolder, string profileSlug);

    /// <summary>
    /// Open a project. Depending on the state of the current AppFlow this will either advance to the LoadingSplash,
    /// or open a popup, asking if the project should be opened in the current or a new window.
    /// </summary>
    public Task OpenProject(string projectPath);

    public FrostyProject? FrostyProject { get; }

    public void RefreshEbxListFromFrosty();

    public IObservable<IChangeSet<EbxAssetEntry, Guid>> ConnectEbxList();

    public IObservable<Change<EbxAssetEntry, Guid>> WatchAssetEntry(Guid key);

}