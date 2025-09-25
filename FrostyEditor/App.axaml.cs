using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Autofac;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels.Windows;
using ProjectWindow = FrostyEditor.Views.Windows.ProjectWindow;

namespace FrostyEditor;

public partial class App : Application
{
    private readonly ILifetimeScope m_scope;
    public static ILifetimeScope? DesignContainer;

    public App(IContainer container)
    {
        m_scope = container.BeginLifetimeScope();

        if (Design.IsDesignMode)
        {
            DesignContainer = m_scope;
        }
    }

    public override void Initialize()
    {
        DataTemplates.Add(m_scope.Resolve<ViewLocator>());
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = m_scope.BeginLifetimeScope().Resolve<ProjectWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}