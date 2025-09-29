using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls;
using FrostyEditor.Utilities;
using FrostyEditor.Views.Windows;

namespace FrostyEditor.Services.Implementation;

public class AppFlowService : IAppFlowService
{
    public required ILifetimeScope ParentScope { private get; init; }

    private ILifetimeScope? m_currentScope;
    private IAppFlowService.FlowState m_currentFlowState = IAppFlowService.FlowState.ProjectScreen;

    public void Init(string? projectPath)
    {
        if (m_currentScope is not null)
        {
            throw new InvalidOperationException("AppFlowService.Init() can only be called once.");
        }

        m_currentFlowState = IAppFlowService.FlowState.ProjectScreen;
        m_currentScope = ParentScope.BeginLifetimeScope(builder =>
        {
            builder.RegisterType<ProjectWindow>().As<Window>().InstancePerLifetimeScope();
        });

        m_currentScope.Resolve<Window>().Show();

        // TODO: Find a way to tell the ProjectWindow that a project should be loaded, so it calls OpenProject in WhenActivated
    }

    public IAppFlowService.FlowState GetCurrentFlowState() => m_currentFlowState;

    public async Task SwitchToLoadingSplash()
    {
        m_currentFlowState = IAppFlowService.FlowState.LoadingSplash;

        var newScope = ParentScope.BeginLifetimeScope(builder =>
        {
            builder.RegisterType<LoadingSplashWindow>().As<Window>().InstancePerLifetimeScope();
        });

        await Async.RunOnUI(async () =>
        {
            var newWindow = newScope.Resolve<Window>();
            m_currentScope!.Resolve<IDialogService>().SwitchOutCurrentWindow(newWindow);
        });

        await m_currentScope!.DisposeAsync();
        m_currentScope = newScope;
    }

    public async Task SwitchToEditor()
    {
        m_currentFlowState = IAppFlowService.FlowState.Editor;

        var newScope = ParentScope.BeginLifetimeScope(builder =>
        {
            builder.RegisterType<EditorWindow>().As<Window>().InstancePerLifetimeScope();
        });

        await Async.RunOnUI(async () =>
        {
            var newWindow = newScope.Resolve<Window>();
            m_currentScope!.Resolve<IDialogService>().SwitchOutCurrentWindow(newWindow);
        });

        await m_currentScope!.DisposeAsync();
        m_currentScope = newScope;
    }
}