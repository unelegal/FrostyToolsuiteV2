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
    /// Only exists in DesignMode. Do not use!
    /// </summary>
    public static IContainer? DesignContainer;

    public App(IContainer container)
    {
        if (Design.IsDesignMode)
        {
            DesignContainer = container;
        }
        else
        {
            m_lifetimeManager = container.Resolve<ILifetimeManager>();
            m_lifetimeManager.ServiceContainer = container;
        }
    }

    public override void Initialize()
    {
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