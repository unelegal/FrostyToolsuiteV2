using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Frosty.ModSupport.Project;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.Services.Implementation;

public partial class ProjectService : IProjectService
{
    public required IAppFlowService AppFlowService { private get; init; }

    private FrostyProject? m_currentProject;

    public string? CreateProject(string modName, string modVersion, string createInFolder, string profileSlug)
    {
        string folder = Path.Combine(createInFolder, modName);
        string fullPath = Path.Combine(folder, modName + ".json");
        Directory.CreateDirectory(folder);

        FrostyProject project = new() { ModName = modName, ModVersion = modVersion, ProjectPath = fullPath, ModProfile = profileSlug };

        File.WriteAllText(fullPath, JsonConvert.SerializeObject(project));

        return fullPath;
    }

    public FrostyProject? FrostyProject => m_currentProject;

    [ReactiveCommand]
    private async Task OpenProject(string projectPath)
    {
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