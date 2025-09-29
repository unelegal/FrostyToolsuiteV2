using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FrostyEditor.ViewModels.Windows;
using ReactiveUI;

namespace FrostyEditor.Views.Windows;

public partial class EditorWindow : ReactiveWindow<EditorWindowViewModel>
{
    public EditorWindow()
    {
        this.WhenActivated(_ => { });
        InitializeComponent();
        WindowState = WindowState.Maximized;
    }

    public EditorWindow(EditorWindowViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}