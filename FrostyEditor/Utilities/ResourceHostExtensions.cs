using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace FrostyEditor.Utilities;

public static class ResourceHostExtensions
{
    public static IServiceProvider GetServiceProvider(this IResourceHost control)
    {
        return (IServiceProvider?)control.FindResource(typeof(IServiceProvider)) ??
               throw new Exception("ServiceProvider is missing");
    }

    public static T CreateInstance<T>(this IResourceHost control) where T : notnull
    {
        return control.GetServiceProvider().GetRequiredService<T>();
    }
}