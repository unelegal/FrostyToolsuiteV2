using System.Collections.Generic;
using System.Threading.Tasks;
using FrostyEditor.Models;

namespace FrostyEditor.Services;

public interface IProfileService
{
    /// <summary>
    /// Get all profile instances. Loads from disk on first call
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<ProfileInstance>> GetProfilesAsync();
}