using System;
using System.IO;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Frosty.Sdk;
using Frosty.Sdk.Managers;
using FrostyEditor.Services;
using Octokit;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Windows;

public partial class LoadingSplashViewModel : ViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    public required IProjectService ProjectService { private get; init; }
    public required IProfileService ProfileService { private get; init; }
    public required IDialogService DialogService { private get; init; }

    public LoadingSplashViewModel()
    {
        this.WhenActivated((CompositeDisposable disposables) =>
        {
        });
    }

    [ReactiveCommand]
    private async Task Load()
    {
        var profile = ProfileService.GetProfileInstance(ProjectService.FrostyProject!.ModProfile);
        if (profile is null)
        {
            throw new Exception("Profile not found");
        }

        if (!ProfilesLibrary.Initialize(profile.ProfileKey))
        {
            throw new Exception("Profile not found");
        }

        if (ProfilesLibrary.RequiresInitFsKey)
        {
            KeyManager.AddKey("InitFsKey", profile.InitFsKey!);
        }

        if (ProfilesLibrary.RequiresBundleKey)
        {
            KeyManager.AddKey("BundleEncryptionKey", profile.BundleKey!);
        }

        if (ProfilesLibrary.RequiresCasKey)
        {
            KeyManager.AddKey("CasObfuscationKey", profile.CasKey!);
        }

        if (!FileSystemManager.Initialize(Path.GetDirectoryName(profile.GamePath)!))
        {
            throw new Exception("Failed to initialize FileSystemManager");
        }

        if (!File.Exists(ProfilesLibrary.SdkPath))
        {
            await DialogService.GenerateSdk.Handle(Unit.Default);
        }

        if (!TypeLibrary.Initialize())
        {
            throw new Exception("Failed to initialize TypeLibrary");
        }

        if (!ResourceManager.Initialize())
        {
            throw new Exception("Failed to initialize ResourceManager");
        }

        if (!AssetManager.Initialize())
        {
            throw new Exception("Failed to initialize AssetManager");
        }
    }

}