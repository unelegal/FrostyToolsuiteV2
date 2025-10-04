using System;
using Dock.Model.Controls;

namespace FrostyEditor.Services;

public interface IDockingService
{
    public IRootDock? CreateDockingRoot();

    public void OpenAsset(Guid guid);
}