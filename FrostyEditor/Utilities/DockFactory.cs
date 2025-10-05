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

    private ToolDock? m_leftToolDock;
    public ToolDock? LeftToolDock => m_leftToolDock;

    private DocumentDock? m_mainAssetDock;
    public DocumentDock? MainAssetDock => m_mainAssetDock;

    private IRootDock? m_rootDock;
    public IRootDock? RootDock => m_rootDock;

    public override IRootDock CreateLayout()
    {
        var dataExplorer = ServiceScope.Resolve<DataExplorerViewModel>();
        var logViewer = ServiceScope.Resolve<LogViewModel>();

        m_leftToolDock = new ToolDock
        {
            VisibleDockables = CreateList<IDockable>(dataExplorer),
            ActiveDockable = dataExplorer,
            Proportion = 0.20,
            IsCollapsable = false
        };
        ToolDock bottomToolDock = new()
        {
            VisibleDockables = CreateList<IDockable>(logViewer),
            ActiveDockable = logViewer,
            Proportion = 0.33
        };
        m_mainAssetDock = new DocumentDock
        {
            VisibleDockables = CreateList<IDockable>(),
            IsCollapsable = false
        };

        ProportionalDock rightVertSplit = new()
        {
            Orientation = Orientation.Vertical,
            VisibleDockables = CreateList<IDockable>(
                m_mainAssetDock,
                new ProportionalDockSplitter(),
                bottomToolDock
            )
        };

        m_rootDock = CreateRootDock();
        m_rootDock.VisibleDockables = CreateList<IDockable>(
            new ProportionalDock
            {
                VisibleDockables = CreateList<IDockable>(
                    m_leftToolDock,
                    new ProportionalDockSplitter(),
                    rightVertSplit
                )
            }
        );
        m_rootDock.DefaultDockable = m_rootDock.VisibleDockables[0];

        return m_rootDock;
    }

    public override void InitLayout(IDockable layout)
    {
        base.InitLayout(layout);
    }
}