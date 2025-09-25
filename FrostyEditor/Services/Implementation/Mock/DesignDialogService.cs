using System.Collections.Generic;
using System.Reactive;
using Avalonia.Platform.Storage;
using FrostyEditor.Views.Windows;
using ReactiveUI;

namespace FrostyEditor.Services.Implementation.Mock;

public class DesignDialogService : IDialogService
{
    public Interaction<Unit, Unit> OpenProfileManager { get; } = new();
    public Interaction<Unit, string?> OpenCreateProject { get; } = new();
    public Interaction<FilePickerOpenOptions, IReadOnlyList<IStorageFile>> OpenFilePicker { get; } = new();
    public Interaction<FolderPickerOpenOptions, IReadOnlyList<IStorageFolder>> OpenFolderPicker { get; } = new();
    public Interaction<Unit, string?> OpenAddProfile { get; } = new();
    public Interaction<Unit, Unit> CloseCurrentWindow { get; } = new();
    public Interaction<object?, Unit> CloseCurrentWindowWithData { get; } = new();

    public DesignDialogService()
    {
        OpenProfileManager.RegisterHandler(ctx => { });

        OpenCreateProject.RegisterHandler(ctx => { });

        OpenFilePicker.RegisterHandler(ctx => { });

        OpenFolderPicker.RegisterHandler(ctx => { });

        OpenAddProfile.RegisterHandler(interaction => { });

        CloseCurrentWindow.RegisterHandler(interaction => { });

        CloseCurrentWindowWithData.RegisterHandler(interaction => { });
    }
}