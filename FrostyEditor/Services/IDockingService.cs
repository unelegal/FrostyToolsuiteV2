using Dock.Model.Controls;

namespace FrostyEditor.Services;

public interface IDockingService
{
    public IRootDock? CreateDockingRoot();
}