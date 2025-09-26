using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace FrostyEditor.Modules;

public class ViewModelModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("ViewModel")).AsSelf();
    }
}