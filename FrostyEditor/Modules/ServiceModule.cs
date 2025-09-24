using Autofac;
using Avalonia.Controls;
using FrostyEditor.Services;
using FrostyEditor.Services.Implementation;
using FrostyEditor.Services.Implementation.Mock;

namespace FrostyEditor.Modules;

public class ServiceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        if (Design.IsDesignMode)
        {
            builder.RegisterType<DesignRecentProjectsService>().As<IRecentProjectsService>().SingleInstance();
            builder.RegisterType<DesignProfileService>().As<IProfileService>().SingleInstance();
        }
        else
        {
            builder.RegisterType<RecentProjectsService>().As<IRecentProjectsService>().SingleInstance();
            builder.RegisterType<ProfileService>().As<IProfileService>().SingleInstance();
        }

        builder.RegisterType<ViewLocator>().AsSelf().SingleInstance();
    }
}