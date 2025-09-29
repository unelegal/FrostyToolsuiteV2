using System;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;

namespace FrostyEditor.ViewModels.Tools;

public partial class DataExplorerViewModel : Tool
{

    public DataExplorerViewModel()
    {
        Id = "DataExplorer";
        Title = "Data Explorer";
        CanClose = false;
    }
}