using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Intrinsics;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using FrostyEditor.Models;
using FrostyEditor.Services;
using FrostyEditor.ViewModels.Data;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Slugify;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace FrostyEditor.ViewModels.Windows;

public partial class NewProfileWindowViewModel : ViewModelBase
{
    private readonly IProfileService m_profileService;

    public required IDialogService DialogService { private get; init; }

    public ProfileInstanceViewModel Profile { get; } = new();

    [ObservableAsProperty]
    private bool m_isValidPath;

    [ObservableAsProperty]
    private bool m_isValidProfileKey;

    [ObservableAsProperty]
    private bool m_requiresCasKey;

    [ObservableAsProperty]
    private bool m_requiresBundleKey;

    [ObservableAsProperty]
    private bool m_requiresInitFsKey;

    [ObservableAsProperty]
    private bool m_requiresAnyKey;

    private readonly IObservable<bool> m_canCreateProfile;
    private readonly IObservable<bool> m_canCheckIfValidProfileKey;

    public NewProfileWindowViewModel(IProfileService profileService)
    {
        m_profileService = profileService;

        m_isValidPathHelper = this
            .WhenAnyValue(x => x.Profile.GamePath)
            .Throttle(TimeSpan.FromMilliseconds(400))
            .Select(File.Exists)
            .ToProperty(this, nameof(IsValidPath), scheduler: RxApp.MainThreadScheduler);

        m_isValidProfileKeyHelper = CheckIfValidProfileCommand.ToProperty(this, nameof(IsValidProfileKey), scheduler: RxApp.MainThreadScheduler);

        m_requiresCasKeyHelper = this
            .WhenAnyValue(x => x.Profile.ProfileKey, x => x.IsValidProfileKey)
            .Where(tuple => tuple.Item2)
            .Select(tuple => tuple.Item1)
            .Select(profileKey => m_profileService.RequiresCasKey(profileKey))
            .ToProperty(this, nameof(RequiresCasKey), scheduler: RxApp.MainThreadScheduler);

        m_requiresBundleKeyHelper = this
            .WhenAnyValue(x => x.Profile.ProfileKey, x => x.IsValidProfileKey)
            .Where(tuple => tuple.Item2)
            .Select(tuple => tuple.Item1)
            .Select(profileKey => m_profileService.RequiresBundleKey(profileKey))
            .ToProperty(this, nameof(RequiresBundleKey), scheduler: RxApp.MainThreadScheduler);

        m_requiresInitFsKeyHelper = this
            .WhenAnyValue(x => x.Profile.ProfileKey, x => x.IsValidProfileKey)
            .Where(tuple => tuple.Item2)
            .Select(tuple => tuple.Item1)
            .Select(profileKey => m_profileService.RequiresInitFsKey(profileKey))
            .ToProperty(this, nameof(RequiresInitFsKey), scheduler: RxApp.MainThreadScheduler);

        m_requiresAnyKeyHelper = this
            .WhenAnyValue(
                x => x.RequiresCasKey, x => x.RequiresBundleKey, x => x.RequiresInitFsKey,
                (cas, bundle, initFs) => cas || bundle || initFs)
            .ToProperty(this, nameof(RequiresAnyKey), scheduler: RxApp.MainThreadScheduler);

        m_canCreateProfile = this.WhenAnyValue(
                x => x.IsValidPath,
                x => x.Profile.Name,
                x => x.Profile.Slug,
                x => x.IsValidProfileKey,
                (validPath, name, slug, validKey) => validPath && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(slug) &&
                                                     validKey)
            .SubscribeOn(RxApp.MainThreadScheduler);

        m_canCheckIfValidProfileKey = this.WhenAnyValue(x => x.Profile.ProfileKey, profileKey => !string.IsNullOrWhiteSpace(profileKey))
            .SubscribeOn(RxApp.MainThreadScheduler);

        this
            .WhenAnyValue(x => x.IsValidPath, x => x.Profile.GamePath)
            .SubscribeOn(RxApp.MainThreadScheduler)
            .Subscribe(tuple => { Profile.ProfileKey = tuple.Item1 ? Path.GetFileNameWithoutExtension(tuple.Item2) : ""; });

        this.WhenAnyValue(x => x.Profile.Name)
            .SubscribeOn(RxApp.MainThreadScheduler)
            .Subscribe(name =>
            {
                SlugHelper helper = new();
                Profile.Slug = helper.GenerateSlug(name);
            });

        this.WhenAnyValue(x => x.Profile.ProfileKey)
            .InvokeCommand(CheckIfValidProfileCommand);

        PickFileCommand.SubscribeOn(RxApp.MainThreadScheduler).Subscribe(path =>
        {
            if (path is null)
            {
                return;
            }

            Profile.GamePath = path;
        });
    }

    [ReactiveCommand(CanExecute = nameof(m_canCreateProfile))]
    private async Task Create(string slug) => await DialogService.CloseCurrentWindowWithData.Handle(slug);

    [ReactiveCommand]
    private async Task Cancel() => await DialogService.CloseCurrentWindow.Handle(Unit.Default);

    [ReactiveCommand]
    private async Task<string?> PickFile()
    {
        var files = await DialogService.OpenFilePicker.Handle(new FilePickerOpenOptions
        {
            Title = "Choose a game",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new("Game Executable") { Patterns = ["*.exe"] }
            ]
        });

        return files.Count > 0 ? files[0].TryGetLocalPath() : null;
    }

    [ReactiveCommand(CanExecute = nameof(m_canCheckIfValidProfileKey))]
    private async Task<bool> CheckIfValidProfile(string profileKey) => m_profileService.IsValidProfileKey(profileKey);
}