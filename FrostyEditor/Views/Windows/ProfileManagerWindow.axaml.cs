using System.Reactive.Disposables;
using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Frosty.Sdk.Profiles;
using FrostyEditor.Models;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels.Windows;
using ReactiveUI;

namespace FrostyEditor.Views.Windows;

public partial class ProfileManagerWindow : ReactiveWindow<ProfileManagerViewModel>
{
    public ProfileManagerWindow()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }

    public ProfileManagerWindow(ProfileManagerViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}