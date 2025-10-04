using System;
using System.IO;
using System.Threading.Tasks;
using DynamicData;
using Frosty.ModSupport.Project;
using Frosty.Sdk.Managers.Entries;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.Services.Implementation.Mock;

public partial class DesignProjectService : IProjectService
{
    private FrostyProject? m_currentProject;

    private readonly SourceCache<EbxAssetEntry, Guid> m_ebxSourceCache = new(x => x.Guid);

    public async Task<string?> CreateProject(string modName, string modVersion, string createInFolder, string profileSlug)
    {
        // TODO: Fill cache
        return null;
    }

    public FrostyProject? FrostyProject => m_currentProject;

    public void RefreshEbxListFromFrosty()
    {

    }

    public IObservable<IChangeSet<EbxAssetEntry, Guid>> ConnectEbxList() => m_ebxSourceCache.Connect();

    public IObservable<Change<EbxAssetEntry, Guid>> WatchAssetEntry(Guid key) => m_ebxSourceCache.Watch(key);

    public async Task OpenProject(string projectPath)
    {

    }
}