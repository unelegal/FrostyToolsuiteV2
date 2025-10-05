using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FrostyEditor.ViewModels.Tools;
using ReactiveUI;

namespace FrostyEditor.Views.Tools;

public partial class LogView : ReactiveUserControl<LogViewModel>
{
    public LogView()
    {
        this.WhenActivated(_ => {});
        InitializeComponent();
    }

    public LogView(LogViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}