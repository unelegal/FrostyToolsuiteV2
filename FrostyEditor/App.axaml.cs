using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels;
using FrostyEditor.ViewModels.Windows;
using FrostyEditor.Views;
using Microsoft.Extensions.DependencyInjection;
using ProjectWindow = FrostyEditor.Views.Windows.ProjectWindow;

namespace FrostyEditor;

public partial class App : Application
{
    private readonly IServiceProvider m_serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        m_serviceProvider = serviceProvider;
    }

    public override void Initialize()
    {
        DataTemplates.Add(m_serviceProvider.GetRequiredService<ViewLocator>());
        AvaloniaXamlLoader.Load(this);
        Resources[typeof(IServiceProvider)] = m_serviceProvider;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new ProjectWindow { DataContext = this.CreateInstance<ProjectWindowViewModel>() };
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