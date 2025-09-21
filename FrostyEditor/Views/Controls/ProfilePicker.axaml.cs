using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FrostyEditor.ViewModels.Controls;
using ReactiveUI;

namespace FrostyEditor.Views.Controls;

public partial class ProfilePicker : ReactiveUserControl<ProfilePickerViewModel>
{
    public ProfilePicker()
    {
        this.WhenActivated(_ => { });
        InitializeComponent();
    }
}