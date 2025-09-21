using System;
using System.IO;
using Newtonsoft.Json;

namespace FrostyEditor.Models;

[JsonObject(MemberSerialization.OptIn)]
public class RecentProjectEntry
{
    public string Name => Path.GetFileNameWithoutExtension(FullPath);

    [JsonProperty]
    public required string FullPath { get; init; }

    [JsonProperty]
    public required DateTime LastOpened { get; init; }
}