using System;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using Avalonia.Controls;
using ReactiveUI;
using Module = Autofac.Module;

namespace FrostyEditor.Modules;

public class ViewModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Window")).AsSelf().InstancePerLifetimeScope();
        builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IViewFor<>)).InstancePerDependency();
    }
}