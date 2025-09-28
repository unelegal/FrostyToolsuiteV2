using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FrostyEditor.ViewModels.Windows;
using ReactiveUI;

namespace FrostyEditor.Views.Windows;

public partial class LoadingSplashWindow : ReactiveWindow<LoadingSplashViewModel>
{
    public LoadingSplashWindow()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }

    public LoadingSplashWindow(LoadingSplashViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}