using System.Reactive.Disposables;
using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FrostyEditor.Models;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels.Windows;
using ReactiveUI;

namespace FrostyEditor.Views.Windows;

public partial class ProfileManagerWindow : ReactiveWindow<ProfileManagerViewModel>
{
    public ProfileManagerWindow()
    {
        this.WhenActivated(disposables =>
        {
            this.ViewModel!.AddProfileInteraction.RegisterHandler(async interaction =>
            {
                var dialogWindow = new NewProfileWindow { DataContext = App.Locator.Resolve<NewProfileWindowViewModel>() };

                interaction.SetOutput(await dialogWindow.ShowDialog<ProfileInstance?>(this));
            }).DisposeWith(disposables);
        });
        InitializeComponent();
    }
}