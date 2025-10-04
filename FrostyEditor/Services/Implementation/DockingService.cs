using System;
using System.Linq;
using Autofac;
using Dock.Model.Controls;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels.Documents;

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

    public void OpenAsset(Guid guid)
    {
        var existing = Factory.MainAssetDock!.VisibleDockables?.FirstOrDefault(d => d.Id == guid.ToString());

        if (existing is not null)
        {
            Factory.MainAssetDock!.ActiveDockable = existing;
            return;
        }

        EbxViewerViewModel doc = ServiceScope.Resolve<EbxViewerViewModel>();
        doc.CurrentGuid = guid;

        Factory.AddDockable(Factory.MainAssetDock!, doc);
        Factory.SetActiveDockable(doc);
        Factory.SetFocusedDockable(Factory.MainAssetDock!, doc);

    }
}