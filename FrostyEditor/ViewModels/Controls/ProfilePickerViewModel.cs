using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FrostyEditor.Models;
using FrostyEditor.Services;
using FrostyEditor.ViewModels.Data;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Tmds.DBus.Protocol;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace FrostyEditor.ViewModels.Controls;

public partial class ProfilePickerViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly IProfileService m_profileService;

    public ViewModelActivator Activator { get; } = new();

    public Interaction<Unit, Unit> OpenProfileManagerInteraction { get; } = new();

    [ObservableAsProperty]
    private IEnumerable<ProfileInstanceViewModel> m_profiles = [];

    [Reactive]
    private ProfileInstanceViewModel? m_selectedProfile;

    public ProfilePickerViewModel(IProfileService profileService)
    {
        m_profileService = profileService;

        m_profilesHelper = LoadProfilesCommand.ToProperty(this, nameof(Profiles), scheduler: RxApp.MainThreadScheduler);

        this.WhenActivated((CompositeDisposable disposables) => { LoadProfilesCommand.Execute().Subscribe(); });
    }

    [ReactiveCommand]
    private async Task<IEnumerable<ProfileInstanceViewModel>> LoadProfiles() => m_profileService.GetProfileInstances().Select(p => new ProfileInstanceViewModel(p));

    [ReactiveCommand]
    private async Task OpenProfileManager()
    {
        await OpenProfileManagerInteraction.Handle(Unit.Default);
        await LoadProfilesCommand.Execute();
    }
}