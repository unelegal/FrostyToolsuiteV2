using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using FrostyEditor.Models;

namespace FrostyEditor.Services.Implementation.Mock;

public class DesignProfileService : IProfileService
{
    private readonly SourceCache<ProfileInstance, string> m_profileInstances = new(t => t.Slug);

    public DesignProfileService()
    {
        m_profileInstances.AddOrUpdate([
            new ProfileInstance { Slug = "battlefield6", Name = "Battlefield 6", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "bf6event" },
            new ProfileInstance { Slug = "bf2042", Name = "Battlefield 2042", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "BF2042" },
            new ProfileInstance { Slug = "whatever1", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever2", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever3", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever4", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever5", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever6", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever7", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" }
        ]);
    }

    public IObservable<IChangeSet<ProfileInstance, string>> ConnectProfiles()
    {
        return m_profileInstances.Connect();
    }

    public void RefreshProfiles()
    {

    }

    public bool AddProfileInstance(ProfileInstance profile)
    {
        m_profileInstances.AddOrUpdate(profile);
        return true;
    }

    public bool RemoveProfileInstance(string slug)
    {
        m_profileInstances.RemoveKey(slug);
        return true;
    }

    public bool IsValidProfileKey(string profileKey)
    {
        return false;
    }

    public bool RequiresCasKey(string profileKey)
    {
        return true;
    }

    public bool RequiresBundleKey(string profileKey)
    {
        return true;
    }

    public bool RequiresInitFsKey(string profileKey)
    {
        return true;
    }
}