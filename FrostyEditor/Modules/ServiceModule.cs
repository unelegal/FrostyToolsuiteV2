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
            builder.RegisterType<DesignProjectService>().As<IProjectService>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
        }
        else
        {
            builder.RegisterType<RecentProjectsService>().As<IRecentProjectsService>().SingleInstance();
            builder.RegisterType<ProfileService>().As<IProfileService>().SingleInstance();
            builder.RegisterType<ProjectService>().As<IProjectService>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }

        builder.RegisterType<ViewLocator>().AsSelf().SingleInstance();
    }
}