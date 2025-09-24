using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrostyEditor.Models;

namespace FrostyEditor.Services.Implementation.Mock;

public class DesignProfileService : IProfileService
{
    private readonly object m_profileListLock = new();
    private readonly HashSet<ProfileInstance> m_profileInstances;

    public DesignProfileService()
    {
        m_profileInstances = new(new ProfileInstance.SlugComparer());

        m_profileInstances.UnionWith([
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

    public IEnumerable<ProfileInstance> GetProfileInstances()
    {
        lock (m_profileListLock)
        {
            return m_profileInstances.ToList();
        }
    }

    public bool AddProfileInstance(ProfileInstance profile)
    {
        lock (m_profileListLock)
        {
            return m_profileInstances.Add(profile);
        }
    }

    public bool RemoveProfileInstance(string slug)
    {
        lock (m_profileListLock)
        {
            return m_profileInstances.RemoveWhere(x => x.Slug == slug) >= 1;
        }
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