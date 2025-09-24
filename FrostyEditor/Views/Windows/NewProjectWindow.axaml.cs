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
        this.WhenActivated(disposables =>
        {
            this.ViewModel!.CloseDialogInteraction.RegisterHandler(interaction =>
            {
                this.Close(interaction.Input);
                interaction.SetOutput(Unit.Default);
            }).DisposeWith(disposables);

            this.ViewModel!.ProfilePickerViewModel.OpenProfileManagerInteraction.RegisterHandler(async interaction =>
            {
                var dialogWindow = new ProfileManagerWindow() { DataContext = App.Locator.Resolve<ProfileManagerViewModel>() };
                await dialogWindow.ShowDialog(this);

                interaction.SetOutput(Unit.Default);
            });

            this.ViewModel!.PickFolderInteraction.RegisterHandler(async interaction =>
            {
                var folders = await GetTopLevel(this)!.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                {
                    Title = "Choose a game",
                    AllowMultiple = false
                });

                interaction.SetOutput(folders.Count >= 1 ? folders[0].TryGetLocalPath() : null);
            });
        });
        InitializeComponent();
    }
}