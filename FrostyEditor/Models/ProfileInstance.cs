using Newtonsoft.Json;

namespace FrostyEditor.Models;

public class ProfileInstance
{
    public string Slug { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string ProfileKey { get; set; } = string.Empty;

    public string GamePath { get; set; } = string.Empty;

    public override string ToString() => Name;
}