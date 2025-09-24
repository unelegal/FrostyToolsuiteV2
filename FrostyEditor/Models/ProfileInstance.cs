using System.Collections.Generic;
using Newtonsoft.Json;
using ReactiveUI;

namespace FrostyEditor.Models;

[JsonObject(MemberSerialization.OptIn)]
public class ProfileInstance
{
    [JsonProperty]
    public string Slug { get; set; } = string.Empty;

    [JsonProperty]
    public string Name { get; set; } = string.Empty;

    [JsonProperty]
    public string ProfileKey { get; set; } = string.Empty;

    [JsonProperty]
    public string GamePath { get; set; } = string.Empty;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public byte[]? CasKey { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public byte[]? BundleKey { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public byte[]? InitFsKey { get; set; }

    public class SlugComparer : IEqualityComparer<ProfileInstance>
    {
        public bool Equals(ProfileInstance? x, ProfileInstance? y)
        {
            if (x is null || y is null)
            {
                return false;
            }

            return x.Slug == y.Slug;
        }

        public int GetHashCode(ProfileInstance obj) => obj.Slug.GetHashCode();
    }
}