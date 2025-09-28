using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Disposables;
using Autofac;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using FrostyEditor.Services;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels.Windows;
using ReactiveUI;

namespace FrostyEditor.Views.Windows;

public partial class ProjectWindow : ReactiveWindow<ProjectWindowViewModel>
{
    public ProjectWindow()
    {
        this.WhenActivated(disposables =>
        {
        });
        InitializeComponent();
    }

    public ProjectWindow(ProjectWindowViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}