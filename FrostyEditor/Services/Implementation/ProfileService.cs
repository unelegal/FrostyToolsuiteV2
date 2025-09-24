using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly HashSet<ProfileInstance> m_profileInstances;
    private bool m_initialized = false;

    public ProfileService()
    {
        m_profileInstances = new(new ProfileInstance.SlugComparer());

        /*m_profileInstances.UnionWith([
            new ProfileInstance { Slug = "battlefield6", Name = "Battlefield 6", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "bf6event" },
            new ProfileInstance { Slug = "bf2042", Name = "Battlefield 2042", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "BF2042" },
            new ProfileInstance { Slug = "whatever1", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever2", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever3", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever4", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever5", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever6", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever7", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" }
        ]);*/

        // TEMP
        Utils.BaseDirectory = Path.GetDirectoryName(AppContext.BaseDirectory) ?? string.Empty;

        ProfilesLibrary.Initialize();
    }

    private void LoadProfileInstancesFromDisk()
    {
        if (!File.Exists(s_profileInstancesFile))
        {
            return;
        }

        lock (m_profileListLock)
        {
            string content = File.ReadAllText(s_profileInstancesFile);
            m_profileInstances.UnionWith(JsonConvert.DeserializeObject<IEnumerable<ProfileInstance>>(content) ?? []);
        }
}

    private void SaveProfileInstancesToDisk()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(s_profileInstancesFile)!);

        lock (m_profileListLock)
        {
            File.WriteAllText(s_profileInstancesFile, JsonConvert.SerializeObject(m_profileInstances.ToList()));
        }
    }

    public IEnumerable<ProfileInstance> GetProfileInstances()
    {
        lock (m_profileListLock)
        {
            if (!m_initialized)
            {
                LoadProfileInstancesFromDisk();
                m_initialized = true;
            }

            return m_profileInstances.ToList();
        }
    }

    public bool AddProfileInstance(ProfileInstance profile)
    {
        lock (m_profileListLock)
        {
            if (m_profileInstances.Add(profile))
            {
                SaveProfileInstancesToDisk();
                return true;
            }

            return false;
        }
    }

    public bool RemoveProfileInstance(string slug)
    {
        lock (m_profileListLock)
        {
            if (m_profileInstances.RemoveWhere(x => x.Slug == slug) >= 1)
            {
                SaveProfileInstancesToDisk();
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
}