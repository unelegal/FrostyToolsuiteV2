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
            builder.RegisterType<DesignDialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<DesignAppFlowService>().As<IAppFlowService>().SingleInstance();
        }
        else
        {
            builder.RegisterType<LifetimeManager>().As<ILifetimeManager>().SingleInstance();
            builder.RegisterType<RecentProjectsService>().As<IRecentProjectsService>().SingleInstance();
            builder.RegisterType<ProfileService>().As<IProfileService>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectService>().As<IProjectService>().InstancePerMatchingLifetimeScope(IAppFlowService.AppFlowTag);
            builder.RegisterType<AppFlowService>().As<IAppFlowService>().InstancePerMatchingLifetimeScope(IAppFlowService.AppFlowTag);
        }

        builder.RegisterType<IAppFlowService.FlowId>().InstancePerMatchingLifetimeScope(IAppFlowService.AppFlowTag);
    }
}