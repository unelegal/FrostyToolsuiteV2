using System;
using Autofac;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Dock.Model.Core;
using Dock.Model.ReactiveUI.Controls;
using FrostyEditor.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;

namespace FrostyEditor;

public class ViewLocator : IDataTemplate
{
    public required ILifetimeScope ContainerScope { private get; init; }

    private IViewFor? Resolve(object viewModel)
    {
        var type = viewModel.GetType();
        var viewForType = typeof(IViewFor<>).MakeGenericType(type);
        if (ContainerScope.Resolve(viewForType) is IViewFor view)
        {
            view.ViewModel = viewModel;
            return view;
        }

        return null;
    }

    private IViewFor? ResolveForMatch(object viewModel)
    {
        var type = viewModel.GetType();
        var viewForType = typeof(IViewFor<>).MakeGenericType(type);
        if (Locator.Current.GetService(viewForType) is IViewFor view)
        {
            return view;
        }

        return null;
    }

    public Control? Build(object? param)
    {
        if (param is null)
        {
            return null;
        }

        if (Resolve(param) is Control control)
        {
            return control;
        }

        string name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        if (data is null)
        {
            return false;
        }

        if (data is IDockable)
        {
            return true;
        }

        return ResolveForMatch(data) is not null;
    }
}