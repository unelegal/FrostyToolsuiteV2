using System.Collections.Generic;
using Autofac;
using Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;
using FrostyEditor.Services;
using FrostyEditor.ViewModels.Tools;
using FrostyEditor.Views.Tools;

namespace FrostyEditor.Utilities;

public class DockFactory : Factory
{
    public required ILifetimeScope ServiceScope { private get; init; }

    public override IRootDock CreateLayout()
    {
        var dataExplorer = ServiceScope.Resolve<DataExplorerViewModel>();

        var root = CreateRootDock();
        root.VisibleDockables = CreateList<IDockable>(
            new ToolDock
            {
                VisibleDockables = CreateList<IDockable>(dataExplorer),
                ActiveDockable = dataExplorer,
            }
        );
        root.DefaultDockable = root.VisibleDockables[0];

        return root;
    }

    public override void InitLayout(IDockable layout)
    {
        base.InitLayout(layout);
    }
}