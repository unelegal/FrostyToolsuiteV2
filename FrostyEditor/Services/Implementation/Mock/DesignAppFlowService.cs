using System.Threading.Tasks;
using Avalonia.Controls;

namespace FrostyEditor.Services.Implementation.Mock;

public class DesignAppFlowService : IAppFlowService
{
    public void Init(string? projectPath)
    {

    }

    public IAppFlowService.FlowState GetCurrentFlowState()
    {
        return IAppFlowService.FlowState.ProjectScreen;
    }

    public async Task SwitchToLoadingSplash()
    {

    }

    public async Task SwitchToEditor()
    {

    }
}