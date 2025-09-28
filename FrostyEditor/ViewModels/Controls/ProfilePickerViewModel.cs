using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using FrostyEditor.Models;
using FrostyEditor.Services;
using FrostyEditor.Utilities;
using FrostyEditor.ViewModels.Data;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Tmds.DBus.Protocol;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace FrostyEditor.ViewModels.Controls;

public partial class ProfilePickerViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly IProfileService m_profileService;

    public required IDialogService DialogService { private get; init; }

    public ViewModelActivator Activator { get; } = new();

    private readonly ReadOnlyObservableCollection<ProfileInstanceViewModel> m_profileInstances;
    public ReadOnlyObservableCollection<ProfileInstanceViewModel> ProfileInstances => m_profileInstances;

    [Reactive]
    private ProfileInstanceViewModel? m_selectedProfile;

    public ProfilePickerViewModel(IProfileService profileService)
    {
        m_profileService = profileService;

        m_profileService.ConnectProfiles()
            .Transform(p => new ProfileInstanceViewModel(p))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out m_profileInstances)
            .Subscribe();

        this.WhenActivated((CompositeDisposable disposables) => { Async.RunInBackground(m_profileService.RefreshProfiles).ConfigureAwait(false); });
    }

    [ReactiveCommand]
    private async Task OpenProfileManager()
    {
        await DialogService.OpenProfileManager();
    }
}