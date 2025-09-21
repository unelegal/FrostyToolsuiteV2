using System.Reactive;
using System.Reactive.Linq;
using FrostyEditor.ViewModels.Controls;
using ReactiveUI;

namespace FrostyEditor.ViewModels.Windows;

public class NewProjectWindowViewModel : ReactiveObject
{
    public ProfilePickerViewModel ProfilePickerViewModel { get; }

    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateCommand { get; }

    public Interaction<string?, Unit> CloseDialogInteraction { get; }

    public NewProjectWindowViewModel(ProfilePickerViewModel profilePickerViewModel)
    {
        ProfilePickerViewModel = profilePickerViewModel;

        CloseDialogInteraction = new Interaction<string?, Unit>();

        CancelCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await CloseDialogInteraction.Handle(null);
        });

        CreateCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await CloseDialogInteraction.Handle(null);
        });
    }



}