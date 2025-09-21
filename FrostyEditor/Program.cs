using Avalonia;
using System;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.ReactiveUI;
using FrostyEditor.Services;
using FrostyEditor.Services.Implementation;
using FrostyEditor.Services.Implementation.Mock;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace FrostyEditor;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() => BuildAvaloniaAppWithServices(BuildServiceProvider());

    private static AppBuilder BuildAvaloniaAppWithServices(IServiceProvider serviceProvider)
        => AppBuilder.Configure(() => new App(serviceProvider))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();

    private static ServiceCollection BuildBaseServiceCollection()
    {
        var builder = new ServiceCollection();
        builder
            .AddEditorBaseServices()
            .AddEditorViewModels();

        return builder;
    }

    private static ServiceProvider BuildDesignServiceProvider()
    {
        ServiceCollection builder = BuildBaseServiceCollection();
        builder
            .AddSingleton<IRecentProjectsService, DesignRecentProjectsService>()
            .AddSingleton<IProfileService, DesignProfileService>();

        return builder.BuildServiceProvider();
    }

    private static ServiceProvider BuildRuntimeServiceProvider()
    {
        ServiceCollection builder = BuildBaseServiceCollection();
        builder
            .AddSingleton<IRecentProjectsService, RecentProjectsService>()
            .AddSingleton<IProfileService, ProfileService>();

        return builder.BuildServiceProvider();
    }

    private static ServiceProvider BuildServiceProvider()
    {
        return Design.IsDesignMode ? BuildDesignServiceProvider() : BuildRuntimeServiceProvider();
    }
}