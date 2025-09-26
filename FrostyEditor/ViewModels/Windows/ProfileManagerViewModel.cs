using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Selection;
using DynamicData;
using FrostyEditor.Models;
using FrostyEditor.Services;
using FrostyEditor.ViewModels.Data;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace FrostyEditor.ViewModels.Windows;

public partial class ProfileManagerViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly IProfileService m_profileService;

    public required IDialogService DialogService { private get; init; }

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private ProfileInstanceViewModel? m_selectedProfile;

    private readonly IObservable<bool> m_canRemoveProfile;

    private readonly ReadOnlyObservableCollection<ProfileInstanceViewModel> m_profileInstances;
    public ReadOnlyObservableCollection<ProfileInstanceViewModel> ProfileInstances => m_profileInstances;

    public ProfileManagerViewModel(IProfileService profileService)
    {
        m_profileService = profileService;

        m_canRemoveProfile = this
            .WhenAnyValue(x => x.SelectedProfile, (ProfileInstanceViewModel? selected) => selected is not null)
            .ObserveOn(RxApp.MainThreadScheduler);

        m_profileService.ConnectProfiles()
            .Transform(p => new ProfileInstanceViewModel(p))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out m_profileInstances)
            .Subscribe();

        this.WhenActivated((CompositeDisposable disposables) => { LoadProfilesCommand.Execute().Subscribe(); });
    }

    [ReactiveCommand]
    private async Task LoadProfiles()
    {
        m_profileService.RefreshProfiles();
    }

    [ReactiveCommand]
    private async Task AddProfile()
    {
        string? newProfileSlug = await DialogService.OpenAddProfile.Handle(Unit.Default);
        if (newProfileSlug is null)
        {
            return;
        }

        // TODO: Change selection
    }

    [ReactiveCommand(CanExecute = nameof(m_canRemoveProfile))]
    private async Task RemoveProfile(ProfileInstanceViewModel? selectedProfile)
    {
        if (selectedProfile is null)
        {
            return;
        }

        // TODO: Are you sure popup
        m_profileService.RemoveProfileInstance(selectedProfile.Slug);
    }
}