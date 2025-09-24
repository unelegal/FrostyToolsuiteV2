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
        this.WhenActivated(disposables =>
        {
            this.ViewModel!.CloseDialogInteraction.RegisterHandler(interaction =>
            {
                this.Close();
                interaction.SetOutput(Unit.Default);
            }).DisposeWith(disposables);

            this.ViewModel!.CloseDialogWithDataIntegration.RegisterHandler(interaction =>
            {
                this.Close(this.ViewModel!.Profile.ToModel());
                interaction.SetOutput(Unit.Default);
            }).DisposeWith(disposables);

            this.ViewModel!.PickFileInteraction.RegisterHandler(async interaction =>
            {
                var files = await GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Choose a game",
                    AllowMultiple = false,
                    FileTypeFilter =
                    [
                        new("Game Executable") { Patterns = ["*.exe"] }
                    ]
                });

                interaction.SetOutput(files.Count >= 1 ? files[0].TryGetLocalPath() : null);
            });
        });
        InitializeComponent();
    }
}