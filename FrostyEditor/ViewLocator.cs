using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using FrostyEditor.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FrostyEditor;

public class ViewLocator : IDataTemplate
{
    private readonly IServiceProvider m_serviceProvider;

    public ViewLocator(IServiceProvider serviceProvider)
    {
        m_serviceProvider = serviceProvider;
    }

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

        IServiceScope scope = m_serviceProvider.CreateScope();
        return (Control) scope.ServiceProvider.GetRequiredService(type);

    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}