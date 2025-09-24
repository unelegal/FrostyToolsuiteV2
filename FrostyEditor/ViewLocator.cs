using System;
using Autofac;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using FrostyEditor.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FrostyEditor;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
        {
            return null;
        }

        string name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        Type? type = Type.GetType(name);

        if (type == null)
        {
            return new TextBlock { Text = "Not Found: " + name };
        }

        return (Control) App.Locator.Resolve(type);
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}