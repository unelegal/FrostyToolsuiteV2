using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using Frosty.Sdk;
using Frosty.Sdk.Utils;
using FrostyEditor.Models;
using Newtonsoft.Json;

namespace FrostyEditor.Services.Implementation;

public class ProfileService : IProfileService
{
    private static readonly string s_profileInstancesFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FrostyEditorV2", "ProfileInstances.json");

    private readonly object m_profileListLock = new();
    private readonly SourceCache<ProfileInstance, string> m_profileInstances = new(t => t.Slug);

    public ProfileService()
    {
        // TEMP
        Utils.BaseDirectory = Path.GetDirectoryName(AppContext.BaseDirectory) ?? string.Empty;

        ProfilesLibrary.Initialize();
    }

    public IObservable<IChangeSet<ProfileInstance, string>> ConnectProfiles()
    {
        // Technically no lock needed, but there's a warning otherwise
        lock (m_profileListLock)
        {
            return m_profileInstances.Connect();
        }
    }

    public void RefreshProfiles()
    {
        LoadProfileInstancesFromDisk();
    }

    private IEnumerable<ProfileInstance> LoadProfileInstancesFromDisk()
    {
        if (!File.Exists(s_profileInstancesFile))
        {
            return [];
        }

        lock (m_profileListLock)
        {
            string content = File.ReadAllText(s_profileInstancesFile);
            var loaded = JsonConvert.DeserializeObject<IList<ProfileInstance>>(content) ?? [];

            m_profileInstances.EditDiff(loaded, EqualityComparer<ProfileInstance>.Default);

            return loaded;
        }
    }

    private void SaveProfileInstancesToDisk(IEnumerable<ProfileInstance> profileInstances)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(s_profileInstancesFile)!);

        lock (m_profileListLock)
        {
            File.WriteAllText(s_profileInstancesFile, JsonConvert.SerializeObject(profileInstances));
            m_profileInstances.EditDiff(profileInstances, EqualityComparer<ProfileInstance>.Default);
        }
    }

    public bool AddProfileInstance(ProfileInstance profile)
    {
        lock (m_profileListLock)
        {
            var profiles = LoadProfileInstancesFromDisk().ToHashSet(new ProfileInstance.SlugComparer());

            if (profiles.Add(profile))
            {
                SaveProfileInstancesToDisk(profiles);
                return true;
            }

            return false;
        }
    }

    public bool RemoveProfileInstance(string slug)
    {
        lock (m_profileListLock)
        {
            var profiles = LoadProfileInstancesFromDisk().ToHashSet(new ProfileInstance.SlugComparer());

            if (profiles.RemoveWhere(p => p.Slug == slug) > 0)
            {
                SaveProfileInstancesToDisk(profiles);
                return true;
            }

            return false;
        }
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

    public ProfileInstance? GetProfileInstance(string slug)
    {
        RefreshProfiles();
        return m_profileInstances.Lookup(slug).HasValue ? m_profileInstances.Lookup(slug).Value : null;
    }
}