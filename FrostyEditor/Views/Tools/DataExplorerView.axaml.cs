using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FrostyEditor.ViewModels.Tools;

namespace FrostyEditor.Views.Tools;

public partial class DataExplorerView : ReactiveUserControl<DataExplorerViewModel>
{
    public DataExplorerView()
    {
        InitializeComponent();
    }
}