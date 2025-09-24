using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Selection;
using FrostyEditor.Models;
using FrostyEditor.Services;
using FrostyEditor.ViewModels.Data;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace FrostyEditor.ViewModels.Windows;

public partial class ProfileManagerViewModel : ViewModelBase, IActivatableViewModel
{
    public required IProfileService ProfileService { private get; init; }

    public ViewModelActivator Activator { get; } = new();

    public Interaction<Unit, ProfileInstance?> AddProfileInteraction { get; } = new();

    [ObservableAsProperty]
    private IEnumerable<ProfileInstanceViewModel> m_profiles = [];

    [Reactive]
    private ProfileInstanceViewModel? m_selectedProfile;

    private readonly IObservable<bool> m_canRemoveProfile;

    public ProfileManagerViewModel()
    {
        m_profilesHelper = LoadProfilesCommand.ToProperty(this, nameof(Profiles), scheduler: RxApp.MainThreadScheduler);

        m_canRemoveProfile = this
            .WhenAnyValue(x => x.SelectedProfile, (ProfileInstanceViewModel? selected) => selected is not null)
            .ObserveOn(RxApp.MainThreadScheduler);

        this.WhenActivated((CompositeDisposable disposables) => { LoadProfilesCommand.Execute().Subscribe(); });
    }

    [ReactiveCommand]
    private async Task<IEnumerable<ProfileInstanceViewModel>> LoadProfiles()
    {
        return ProfileService.GetProfileInstances().Select(x => new ProfileInstanceViewModel(x));
    }

    [ReactiveCommand]
    private async Task AddProfile()
    {
        ProfileInstance? newProfile = await AddProfileInteraction.Handle(Unit.Default);
        if (newProfile is null)
        {
            return;
        }

        ProfileService.AddProfileInstance(newProfile);
        await LoadProfilesCommand.Execute();
    }

    [ReactiveCommand(CanExecute = nameof(m_canRemoveProfile))]
    private async Task RemoveProfile(ProfileInstanceViewModel? selectedProfile)
    {
        if (selectedProfile is null)
        {
            return;
        }

        // TODO: Are you sure popup
        ProfileService.RemoveProfileInstance(selectedProfile.Slug);
        await LoadProfilesCommand.Execute();
    }
}