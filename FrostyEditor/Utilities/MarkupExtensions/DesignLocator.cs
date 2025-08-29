using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace FrostyEditor.Utilities.MarkupExtensions;

public class DesignLocator(Type type) : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Design locator is only available in Design Mode.");
        }

        return Application.Current!.GetServiceProvider().GetRequiredService(type);
    }
}