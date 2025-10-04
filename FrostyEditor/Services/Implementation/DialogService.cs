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

    public DialogService(Lazy<Window> owningWindow)
    {
        m_lazyOwningWindow = owningWindow;
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

    public async Task OpenProfileManager()
    {
        await ShowDialogAsync<ProfileManagerWindow>();
    }

    public async Task<string?> OpenCreateProject()
    {
        return await ShowDialogAsync<NewProjectWindow, string?>();
    }

    public async Task<IReadOnlyList<IStorageFile>> OpenFilePicker(FilePickerOpenOptions options)
    {
        return await m_owningWindow.StorageProvider.OpenFilePickerAsync(options);
    }

    public async Task<IReadOnlyList<IStorageFolder>> OpenFolderPicker(FolderPickerOpenOptions options)
    {
        return await m_owningWindow.StorageProvider.OpenFolderPickerAsync(options);
    }

    public async Task<string?> OpenAddProfile()
    {
        return await ShowDialogAsync<NewProfileWindow, string?>();
    }

    public void CloseCurrentWindow(object? data = null)
    {
        m_owningWindow.Close(data);
    }

    public void SwitchOutCurrentWindow(Window newWindow)
    {
        newWindow.Show();
        m_owningWindow.Close();
    }

    public async Task<int?> OpenSelectProcess()
    {
        return await ShowDialogAsync<ProcessSelectorWindow, int?>();
    }
}