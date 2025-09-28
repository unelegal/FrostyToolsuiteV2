using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ReactiveUI;

namespace FrostyEditor.Services;

public interface IDialogService
{
    public Task OpenProfileManager();

    public Task<string?> OpenCreateProject();

    public Task<IReadOnlyList<IStorageFile>> OpenFilePicker(FilePickerOpenOptions options);

    public Task<IReadOnlyList<IStorageFolder>> OpenFolderPicker(FolderPickerOpenOptions options);

    /// <summary>
    ///
    /// </summary>
    /// <returns>The Slug of the created ProfileInstance</returns>
    public Task<string?> OpenAddProfile();

    public void CloseCurrentWindow(object? data = null);

    public void SwitchOutCurrentWindow(Window newWindow);

    public Task OpenGenerateSdk();
}