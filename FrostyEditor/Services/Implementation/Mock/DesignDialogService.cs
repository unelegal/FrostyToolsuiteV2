using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using FrostyEditor.Views.Windows;
using ReactiveUI;

namespace FrostyEditor.Services.Implementation.Mock;

public class DesignDialogService : IDialogService
{
    public async Task OpenProfileManager()
    {

    }

    public async Task<string?> OpenCreateProject()
    {
        return null;
    }

    public async Task<IReadOnlyList<IStorageFile>> OpenFilePicker(FilePickerOpenOptions options)
    {
        return [];
    }

    public async Task<IReadOnlyList<IStorageFolder>> OpenFolderPicker(FolderPickerOpenOptions options)
    {
        return [];
    }

    public async Task<string?> OpenAddProfile()
    {
        return null;
    }

    public void CloseCurrentWindow(object? data = null)
    {

    }

    public void SwitchOutCurrentWindow(Window newWindow)
    {

    }

    public async Task<int?> OpenSelectProcess()
    {
        return null;
    }
}