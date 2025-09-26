using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using FrostyEditor.Views.Windows;
using ReactiveUI;

namespace FrostyEditor.Services.Implementation;

public class DialogService : IDialogService
{
    public required ILifetimeScope ServiceScope { private get; init; }

    private readonly Lazy<Window> m_lazyOwningWindow;
    private Window m_owningWindow => m_lazyOwningWindow.Value;

    public Interaction<Unit, Unit> OpenProfileManager { get; } = new();
    public Interaction<Unit, string?> OpenCreateProject { get; } = new();
    public Interaction<FilePickerOpenOptions, IReadOnlyList<IStorageFile>> OpenFilePicker { get; } = new();
    public Interaction<FolderPickerOpenOptions, IReadOnlyList<IStorageFolder>> OpenFolderPicker { get; } = new();
    public Interaction<Unit, string?> OpenAddProfile { get; } = new();
    public Interaction<Unit, Unit> CloseCurrentWindow { get; } = new();
    public Interaction<object?, Unit> CloseCurrentWindowWithData { get; } = new();

    public DialogService(Lazy<Window> owningWindow)
    {
        m_lazyOwningWindow = owningWindow;

        OpenProfileManager.RegisterHandler(async ctx =>
        {
            await ShowDialogAsync<ProfileManagerWindow>();
            ctx.SetOutput(Unit.Default);
        });

        OpenCreateProject.RegisterHandler(async ctx =>
        {
            string? path = await ShowDialogAsync<NewProjectWindow, string?>();
            ctx.SetOutput(path);
        });

        OpenFilePicker.RegisterHandler(async ctx =>
        {
            var files = await m_owningWindow.StorageProvider.OpenFilePickerAsync(ctx.Input);

            ctx.SetOutput(files);
        });

        OpenFolderPicker.RegisterHandler(async ctx =>
        {
            var folders = await m_owningWindow.StorageProvider.OpenFolderPickerAsync(ctx.Input);

            // var folders = await GetTopLevel(this)!.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            //{
            //    Title = "Choose a game",
            //    AllowMultiple = false
            //});
//
            //interaction.SetOutput(folders.Count >= 1 ? folders[0].TryGetLocalPath() : null);

            ctx.SetOutput(folders);
        });

        OpenAddProfile.RegisterHandler(async interaction =>
        {
            string? slug = await ShowDialogAsync<NewProfileWindow, string?>();
            interaction.SetOutput(slug);
        });

        CloseCurrentWindow.RegisterHandler(interaction =>
        {
            m_owningWindow.Close();
            interaction.SetOutput(Unit.Default);
        });

        CloseCurrentWindowWithData.RegisterHandler(interaction =>
        {
            m_owningWindow.Close(interaction.Input);
            interaction.SetOutput(Unit.Default);
        });
    }

    private async Task<TRet> ShowDialogAsync<TWindow, TRet>() where TWindow : Window
    {
        await using var scope = ServiceScope.BeginLifetimeScope(builder =>
        {
            builder.RegisterType<TWindow>().As<Window>().InstancePerLifetimeScope();
        });
        var dialog = scope.Resolve<Window>();
        return await dialog.ShowDialog<TRet>(m_owningWindow);
    }

    private async Task ShowDialogAsync<TWindow>() where TWindow : Window
    {
        await using var scope = ServiceScope.BeginLifetimeScope(builder =>
        {
            builder.RegisterType<TWindow>().As<Window>().InstancePerLifetimeScope();
        });
        var dialog = scope.Resolve<Window>();
        await dialog.ShowDialog(m_owningWindow);
    }
}