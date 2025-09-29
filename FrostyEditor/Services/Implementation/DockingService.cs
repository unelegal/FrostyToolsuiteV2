using Autofac;
using Dock.Model.Controls;
using FrostyEditor.Utilities;

namespace FrostyEditor.Services.Implementation;

public class DockingService : IDockingService
{
    public required ILifetimeScope ServiceScope { private get; init; }
    public required DockFactory Factory { private get; init; }

    public IRootDock? CreateDockingRoot()
    {
        var layout = Factory.CreateLayout();
        Factory.InitLayout(layout);

        return layout;
    }
}