using Avalonia;
using System;
using System.Reflection;
using Autofac;
using Avalonia.ReactiveUI;

namespace FrostyEditor;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        BuildServiceContainer();
        return AppBuilder.Configure(() => new App(BuildServiceContainer()))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }

    private static IContainer BuildServiceContainer()
    {
        ContainerBuilder builder = new();
        Assembly assembly = Assembly.GetExecutingAssembly();
        builder.RegisterAssemblyModules(assembly);

        return builder.Build();
    }
}