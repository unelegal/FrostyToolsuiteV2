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
        OpenProfileManager.RegisterHandler(ctx =>
        {
            ctx.SetOutput(Unit.Default);
        });

        OpenCreateProject.RegisterHandler(ctx =>
        {
            ctx.SetOutput("C:\\Path\\To\\Project\\project.json");
        });

        OpenFilePicker.RegisterHandler(ctx =>
        {
            ctx.SetOutput([]);
        });

        OpenFolderPicker.RegisterHandler(ctx =>
        {
            ctx.SetOutput([]);
        });

        OpenAddProfile.RegisterHandler(ctx =>
        {
            ctx.SetOutput(null);
        });

        CloseCurrentWindow.RegisterHandler(ctx =>
        {
            ctx.SetOutput(Unit.Default);
        });

        CloseCurrentWindowWithData.RegisterHandler(ctx =>
        {
            ctx.SetOutput(Unit.Default);
        });
    }
}