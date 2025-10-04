using System;
using Dock.Model.Controls;

namespace FrostyEditor.Services.Implementation.Mock;

public class DesignDockingService : IDockingService
{
    public IRootDock? CreateDockingRoot()
    {
        return null;
    }

    public void OpenAsset(Guid guid)
    {

    }
}