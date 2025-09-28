using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ReactiveUI;

namespace FrostyEditor.Services;

public interface IDialogService
{
    public Interaction<Unit, Unit> OpenProfileManager { get; }

    public Interaction<Unit, string?> OpenCreateProject { get; }

    public Interaction<FilePickerOpenOptions, IReadOnlyList<IStorageFile>> OpenFilePicker { get; }

    public Interaction<FolderPickerOpenOptions, IReadOnlyList<IStorageFolder>> OpenFolderPicker { get; }

    /// <summary>
    /// Returns the Slug of the created ProfileInstance
    /// </summary>
    public Interaction<Unit, string?> OpenAddProfile { get; }

    public Interaction<Unit, Unit> CloseCurrentWindow { get; }
    public Interaction<object?, Unit> CloseCurrentWindowWithData { get; }

    public Interaction<Window, Unit> SwitchOutCurrentWindow { get; }
    public Interaction<Unit, Unit> GenerateSdk { get; }
}