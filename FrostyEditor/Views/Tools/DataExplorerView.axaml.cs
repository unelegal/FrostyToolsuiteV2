using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FrostyEditor.Models.DataExplorer;
using FrostyEditor.ViewModels.Tools;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace FrostyEditor.Views.Tools;

public partial class DataExplorerView : ReactiveUserControl<DataExplorerViewModel>
{
    public DataExplorerView()
    {
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            ((TreeDataGrid)Tree)
                .Events()
                .DoubleTapped
                .Select(e =>
                {
                    var tappedControl = e.Source as Control;
                    var tappedVm = tappedControl?.DataContext as DataExplorerEntry;

                    var selectedVm = Tree.RowSelection?.SelectedItem as DataExplorerEntry;
                    return selectedVm ?? tappedVm;
                })
                .WhereNotNull()
                .Select(entry => entry.Guid)
                .WhereNotNull()
                .InvokeCommand(ViewModel!.OpenAssetCommand)
                .DisposeWith(disposables);
        });
        InitializeComponent();
    }
}