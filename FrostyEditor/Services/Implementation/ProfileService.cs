using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using Frosty.Sdk;
using Frosty.Sdk.Utils;
using FrostyEditor.Models;
using FrostyEditor.Utilities;
using Newtonsoft.Json;

namespace FrostyEditor.Services.Implementation;

public class ProfileService : IProfileService
{
    private static readonly string s_profileInstancesFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FrostyEditorV2", "ProfileInstances.json");

    private readonly AsyncSemaphore m_fileSemaphore = new(1, 1);
    private readonly SourceCache<ProfileInstance, string> m_profileInstances = new(t => t.Slug);

    public ProfileService(ILoggingService loggingService)
    {
        // TEMP
        Utils.BaseDirectory = Path.GetDirectoryName(AppContext.BaseDirectory) ?? string.Empty;
        FrostyLogger.Logger = loggingService;

        ProfilesLibrary.Initialize();
    }

    public IObservable<IChangeSet<ProfileInstance, string>> ConnectProfiles()
    {
        return m_profileInstances.Connect();
    }

    public async Task RefreshProfiles()
    {
        using var _ = await m_fileSemaphore.WaitAsync();
        await LoadProfileInstancesFromDisk();
    }

    public async Task<bool> AddProfileInstance(ProfileInstance profile)
    {
        using var _ = await m_fileSemaphore.WaitAsync();

        var profiles = (await LoadProfileInstancesFromDisk()).ToHashSet(new ProfileInstance.SlugComparer());

        if (profiles.Add(profile))
        {
            await SaveProfileInstancesToDisk(profiles);
            return true;
        }

        return false;
    }

    public async Task<bool> RemoveProfileInstance(string slug)
    {
        using var _ = await m_fileSemaphore.WaitAsync();

        var profiles = (await LoadProfileInstancesFromDisk()).ToHashSet(new ProfileInstance.SlugComparer());

        if (profiles.RemoveWhere(p => p.Slug == slug) > 0)
        {
            await SaveProfileInstancesToDisk(profiles);
            return true;
        }

        return false;
    }

    public bool IsValidProfileKey(string profileKey)
    {
        return ProfilesLibrary.HasProfile(profileKey);
    }

    public bool RequiresCasKey(string profileKey)
    {
        return ProfilesLibrary.Profiles.Where(profile => profile.Name.Equals(profileKey, StringComparison.OrdinalIgnoreCase)).Select(profile => profile.RequiresCasKey)
            .FirstOrDefault(false);
    }

    public bool RequiresBundleKey(string profileKey)
    {
        return ProfilesLibrary.Profiles.Where(profile => profile.Name.Equals(profileKey, StringComparison.OrdinalIgnoreCase)).Select(profile => profile.RequiresBundleKey)
            .FirstOrDefault(false);
    }

    public bool RequiresInitFsKey(string profileKey)
    {
        return ProfilesLibrary.Profiles.Where(profile => profile.Name.Equals(profileKey, StringComparison.OrdinalIgnoreCase)).Select(profile => profile.RequiresInitFsKey)
            .FirstOrDefault(false);
    }

    public async Task<ProfileInstance?> GetProfileInstance(string slug)
    {
        await RefreshProfiles();
        return m_profileInstances.Lookup(slug).HasValue ? m_profileInstances.Lookup(slug).Value : null;
    }

    /// <summary>
    /// Only call with m_fileSemaphore acquired!
    /// </summary>
    private async Task<IEnumerable<ProfileInstance>> LoadProfileInstancesFromDisk()
    {
        if (!File.Exists(s_profileInstancesFile))
        {
            return [];
        }

        string content = await File.ReadAllTextAsync(s_profileInstancesFile);
        var loaded = JsonConvert.DeserializeObject<IList<ProfileInstance>>(content) ?? [];

        m_profileInstances.EditDiff(loaded, EqualityComparer<ProfileInstance>.Default);

        return loaded;
    }

    /// <summary>
    /// Only call with m_fileSemaphore acquired!
    /// </summary>
    private async Task SaveProfileInstancesToDisk(IEnumerable<ProfileInstance> profileInstances)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(s_profileInstancesFile)!);

        await File.WriteAllTextAsync(s_profileInstancesFile, JsonConvert.SerializeObject(profileInstances));
        m_profileInstances.EditDiff(profileInstances, EqualityComparer<ProfileInstance>.Default);
    }
}