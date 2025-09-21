using FrostyEditor.ViewModels.Controls;
using FrostyEditor.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace FrostyEditor.Utilities;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEditorBaseServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<ViewLocator>();
    }

    public static IServiceCollection AddEditorViewModels(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddTransient<ProjectWindowViewModel>()
            .AddTransient<RecentProjectsPaneViewModel>()
            .AddTransient<NewProjectWindowViewModel>()
            .AddTransient<ProfilePickerViewModel>();
    }
}