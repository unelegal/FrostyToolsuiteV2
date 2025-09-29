using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Autofac;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FrostyEditor.Services;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels.Windows;
using ProjectWindow = FrostyEditor.Views.Windows.ProjectWindow;

namespace FrostyEditor;

public class App : Application
{
    private readonly ILifetimeManager? m_lifetimeManager;

    /// <summary>
    /// Do not use this!
    /// </summary>
    public static IContainer? DesignContainer;

    public App(IContainer container)
    {
        DesignContainer = container;

        if (!Design.IsDesignMode)
        {
            m_lifetimeManager = container.Resolve<ILifetimeManager>();
            m_lifetimeManager.ServiceContainer = container;
        }
    }

    public override void Initialize()
    {
        DataTemplates.Add(DesignContainer!.Resolve<ViewLocator>());
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            m_lifetimeManager!.CreateAppFlow();
            desktop.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }

        base.OnFrameworkInitializationCompleted();
    }
}