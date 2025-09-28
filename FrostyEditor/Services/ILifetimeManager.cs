
using Autofac;

namespace FrostyEditor.Services;

public interface ILifetimeManager
{
    public IContainer? ServiceContainer { set; }

    public IAppFlowService CreateAppFlow(string? projectPath = null);

    public bool DestroyAppFlow(IAppFlowService.FlowId flowId);
}