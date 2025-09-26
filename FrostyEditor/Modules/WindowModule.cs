using System.Reflection;
using Autofac;
using Avalonia.Controls;
using Module = Autofac.Module;

namespace FrostyEditor.Modules;

public class WindowModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Window")).AsSelf().InstancePerLifetimeScope();
    }
}