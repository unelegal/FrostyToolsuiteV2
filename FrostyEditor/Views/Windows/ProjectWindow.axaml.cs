using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Disposables;
using Autofac;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
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
            this.ViewModel!.CreateProjectInteraction.RegisterHandler(async interaction =>
            {
                var dialogWindow = new NewProjectWindow { DataContext = App.Locator.Resolve<NewProjectWindowViewModel>() };

                interaction.SetOutput(await dialogWindow.ShowDialog<string?>(this));
            }).DisposeWith(disposables);

            this.ViewModel!.OpenProfileManagerInteraction.RegisterHandler(async interaction =>
            {
                var dialogWindow = new ProfileManagerWindow() { DataContext = App.Locator.Resolve<ProfileManagerViewModel>() };

                await dialogWindow.ShowDialog(this);
                interaction.SetOutput(Unit.Default);
            }).DisposeWith(disposables);

            this.ViewModel!.OpenKeyManagerInteraction.RegisterHandler(async interaction =>
            {
                // TODO
                interaction.SetOutput(Unit.Default);
            }).DisposeWith(disposables);

            this.ViewModel!.OpenProjectInteraction.RegisterHandler(async interaction =>
            {
                var files = await GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = Assets.Lang.Resources.OpenProject,
                    AllowMultiple = false,
                    FileTypeFilter =
                    [
                        new(Assets.Lang.Resources.FrostyProject) { Patterns = ["*.json"] }
                    ]
                });

                interaction.SetOutput(files.Count >= 1 ? files[0].TryGetLocalPath() : null);
            }).DisposeWith(disposables);
        });
        InitializeComponent();
    }
}