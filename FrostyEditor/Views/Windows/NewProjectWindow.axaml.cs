using System.Reactive;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FrostyEditor.ViewModels.Windows;
using ReactiveUI;

namespace FrostyEditor.Views.Windows;

public partial class NewProjectWindow : ReactiveWindow<NewProjectWindowViewModel>
{
    public NewProjectWindow()
    {
        this.WhenActivated(disposables =>
        {
            this.ViewModel!.CloseDialogInteraction.RegisterHandler(interaction =>
            {
                this.Close(interaction.Input);
                interaction.SetOutput(Unit.Default);
            }).DisposeWith(disposables);
        });
        InitializeComponent();
    }
}