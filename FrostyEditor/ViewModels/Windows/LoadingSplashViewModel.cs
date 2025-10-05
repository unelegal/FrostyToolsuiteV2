using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Frosty.Sdk;
using Frosty.Sdk.Managers;
using Frosty.Sdk.Sdk;
using FrostyEditor.Services;
using FrostyEditor.Utilities;
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
    public required IAppFlowService AppFlowService { private get; init; }

    public LoadingSplashViewModel()
    {
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            Async.RunInBackground(async () =>
            {
                var profile = await ProfileService!.GetProfileInstance(ProjectService!.FrostyProject!.ModProfile);
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
                    int? pid = await Async.RunOnUI(async () => await DialogService!.OpenSelectProcess());
                    if (pid is null)
                    {
                        throw new Exception("No process selected!");
                    }

                    TypeSdkGenerator generator = new();
                    if (!generator.DumpTypes(Process.GetProcessById(pid.Value)))
                    {
                        throw new Exception("Failed to dump types!");
                    }

                    if (!generator.CreateSdk(ProfilesLibrary.SdkPath))
                    {
                        throw new Exception("Failed to generate SDK!");
                    }
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

                ProjectService.RefreshEbxListFromFrosty();

                await AppFlowService!.SwitchToEditor();
            }).ToObservable().Subscribe(_ => { }, onError:
            error =>
            {
                Console.WriteLine(error);
            }).DisposeWith(disposables);
        });
    }

}