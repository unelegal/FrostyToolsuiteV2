using System.Reactive;
using System.Reactive.Disposables;
using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels.Windows;
using ReactiveUI;

namespace FrostyEditor.Views.Windows;

public partial class NewProjectWindow : ReactiveWindow<NewProjectWindowViewModel>
{
    public NewProjectWindow()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }

    public NewProjectWindow(NewProjectWindowViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}