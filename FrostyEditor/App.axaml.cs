using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Autofac;
using Avalonia.Markup.Xaml;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels.Windows;
using ProjectWindow = FrostyEditor.Views.Windows.ProjectWindow;

namespace FrostyEditor;

public partial class App : Application
{
    public static IContainer Locator { get; private set; }

    public App(IContainer container)
    {
        Locator = container;
    }

    public override void Initialize()
    {
        DataTemplates.Add(Locator.Resolve<ViewLocator>());
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new ProjectWindow { DataContext = Locator.Resolve<ProjectWindowViewModel>() };
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