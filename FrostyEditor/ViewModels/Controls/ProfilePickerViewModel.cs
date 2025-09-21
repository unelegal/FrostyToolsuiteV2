using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FrostyEditor.Models;
using FrostyEditor.Services;
using ReactiveUI;

namespace FrostyEditor.ViewModels.Controls;

public class ProfilePickerViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly IProfileService m_profileService;

    public ViewModelActivator Activator { get; }

    private readonly ObservableAsPropertyHelper<IEnumerable<ProfileInstance>> m_profiles;
    public IEnumerable<ProfileInstance> Profiles => m_profiles.Value;
    public ReactiveCommand<Unit, IEnumerable<ProfileInstance>> LoadProfiles { get; }
    public ReactiveCommand<Unit, Unit> OpenProfileManager { get; }
    public Interaction<Unit, Unit> OpenProfileManagerInteraction { get; }

    private ProfileInstance? m_selectedProfile;
    public ProfileInstance? SelectedProfile
    {
        get => m_selectedProfile;
        set => this.RaiseAndSetIfChanged(ref m_selectedProfile, value);
    }

    public ProfilePickerViewModel(IProfileService profileService)
    {
        m_profileService = profileService;
        Activator = new ViewModelActivator();

        LoadProfiles = ReactiveCommand.CreateFromTask(m_profileService.GetProfilesAsync);
        m_profiles = LoadProfiles.ToProperty(this, nameof(Profiles), scheduler: RxApp.MainThreadScheduler);

        OpenProfileManagerInteraction = new Interaction<Unit, Unit>();
        OpenProfileManager = ReactiveCommand.CreateFromTask(async () =>
        {
            await OpenProfileManagerInteraction.Handle(Unit.Default);
        });

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            LoadProfiles.Execute().Subscribe();
        });
    }
}