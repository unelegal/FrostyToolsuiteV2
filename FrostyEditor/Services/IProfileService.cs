using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicData;
using FrostyEditor.Models;

namespace FrostyEditor.Services;

public interface IProfileService
{
    /// <summary>
    /// Get an observable ChangeSet of the loaded Profiles
    /// </summary>
    /// <returns></returns>
    public IObservable<IChangeSet<ProfileInstance, string>> ConnectProfiles();

    /// <summary>
    /// Refreshes the profile instances from disk. Make sure to only call from the thread pool!
    /// </summary>
    public void RefreshProfiles();

    /// <summary>
    /// Add a new profile instance. Writes to disk, make sure to only
    /// call this from the thread pool!
    /// </summary>
    /// <param name="profile">The profile instance to add</param>
    /// <returns>True on success, false if a profile with the same slug already exists</returns>
    public bool AddProfileInstance(ProfileInstance profile);

    /// <summary>
    /// Delete a profile instance. Writes to disk, make sure to only
    /// call this from the thread pool!
    /// </summary>
    /// <param name="slug">Slug of the instance to remove</param>
    /// <returns>True on success, false if the slug could not be found</returns>
    public bool RemoveProfileInstance(string slug);

    /// <summary>
    /// Check if a given profile key is valid
    /// </summary>
    /// <param name="profileKey">The profile key to check</param>
    /// <returns>True if valid</returns>
    public bool IsValidProfileKey(string profileKey);

    /// <summary>
    /// Check if the given profile requires a CAS key
    /// </summary>
    /// <param name="profileKey">The profile key to check</param>
    /// <returns>True if a CAS key is required</returns>
    public bool RequiresCasKey(string profileKey);

    /// <summary>
    /// Check if the given profile requires a Bundle key
    /// </summary>
    /// <param name="profileKey">The profile key to check</param>
    /// <returns>True if a Bundle key is required</returns>
    public bool RequiresBundleKey(string profileKey);

    /// <summary>
    /// Check if the given profile requires an InitFS key
    /// </summary>
    /// <param name="profileKey">The profile key to check</param>
    /// <returns>True if a InitFS key is required</returns>
    public bool RequiresInitFsKey(string profileKey);

    public ProfileInstance? GetProfileInstance(string slug);
}