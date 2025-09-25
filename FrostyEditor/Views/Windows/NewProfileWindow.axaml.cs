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
    public NewProfileWindow(NewProfileWindowViewModel viewModel)
    {
        DataContext = viewModel;

        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}