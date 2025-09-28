using System;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Frosty.ModSupport.Project;
using FrostyEditor.Utilities;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.Services.Implementation;

public partial class ProjectService : IProjectService
{
    public required IAppFlowService AppFlowService { private get; init; }

    private FrostyProject? m_currentProject;

    private readonly AsyncSemaphore m_openProjectSem = new(1, 1);

    public async Task<string?> CreateProject(string modName, string modVersion, string createInFolder, string profileSlug)
    {
        string folder = Path.Combine(createInFolder, modName);
        string fullPath = Path.Combine(folder, modName + ".json");
        Directory.CreateDirectory(folder);

        FrostyProject project = new() { ModName = modName, ModVersion = modVersion, ProjectPath = fullPath, ModProfile = profileSlug };

        await File.WriteAllTextAsync(fullPath, JsonConvert.SerializeObject(project));

        return fullPath;
    }

    public FrostyProject? FrostyProject => m_currentProject;

    // TODO: Move all this logic into AppFlowService
    public async Task OpenProject(string projectPath)
    {
        using var _ = await m_openProjectSem.WaitAsync();

        if (m_currentProject is null)
        {
            if (AppFlowService.GetCurrentFlowState() != IAppFlowService.FlowState.ProjectScreen)
            {
                throw new InvalidOperationException("Flow state does not match current project state!");
            }

            FrostyProject? project = await FrostyProject.LoadAsync(projectPath);
            if (project is null)
            {
                // TODO: Popup for failed project opening
                return;
            }

            m_currentProject = project;
            await AppFlowService.SwitchToLoadingSplash();
            return;
        }

        // TODO: Popup for project opening
        return;
    }
}