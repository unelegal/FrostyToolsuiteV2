using System.Reactive;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using FrostyEditor.ViewModels.Windows;
using ReactiveUI;

namespace FrostyEditor.Views.Windows;

public partial class NewProfileWindow : ReactiveWindow<NewProfileWindowViewModel>
{
    public NewProfileWindow()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }

    public NewProfileWindow(NewProfileWindowViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}