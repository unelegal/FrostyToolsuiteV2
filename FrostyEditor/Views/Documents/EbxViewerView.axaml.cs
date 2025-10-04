using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FrostyEditor.ViewModels.Documents;
using ReactiveUI;

namespace FrostyEditor.Views.Documents;

public partial class EbxViewerView : ReactiveUserControl<EbxViewerViewModel>
{
    public EbxViewerView()
    {
        this.WhenActivated(_ => { });
        InitializeComponent();
    }
}