using System;
using System.Collections.Generic;
using Autofac;

namespace FrostyEditor.Services.Implementation;

public class LifetimeManager : ILifetimeManager
{
    private readonly Dictionary<IAppFlowService.FlowId, ILifetimeScope> m_lifetimes = new();

    public IContainer? ServiceContainer { get; set; }

    public IAppFlowService CreateAppFlow(string? projectPath)
    {
        var scope = ServiceContainer!.BeginLifetimeScope(IAppFlowService.AppFlowTag);
        m_lifetimes.Add(scope.Resolve<IAppFlowService.FlowId>(), scope);

        var flow = scope.Resolve<IAppFlowService>();
        flow.Init(projectPath);
        return flow;
    }

    public bool DestroyAppFlow(IAppFlowService.FlowId flowId)
    {
        if (!m_lifetimes.TryGetValue(flowId, out ILifetimeScope? value))
        {
            return false;
        }

        value.Dispose();
        return m_lifetimes.Remove(flowId);
    }

}