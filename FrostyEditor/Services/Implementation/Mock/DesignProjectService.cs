using System;
using System.IO;
using System.Threading.Tasks;
using Frosty.ModSupport.Project;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.Services.Implementation.Mock;

public partial class DesignProjectService : IProjectService
{
    private FrostyProject? m_currentProject;

    public string? CreateProject(string modName, string modVersion, string createInFolder, string profileSlug)
    {
        return null;
    }

    public FrostyProject? FrostyProject => m_currentProject;

    [ReactiveCommand]
    private async Task OpenProject(string projectPath)
    {

    }
}