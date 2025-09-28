using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Frosty.ModSupport.Project;

[JsonObject(MemberSerialization.OptIn)]
public class FrostyProject : FrostyProjectBase
{
    public string ProjectPath { get; set; } = string.Empty;

    public string? ProjectDirectory => Path.GetDirectoryName(ProjectPath);

    [JsonProperty]
    public string ModProfile { get; set; } =  string.Empty;

    [JsonProperty]
    public string ModName { get; set; } = "Unnamed Mod";

    [JsonProperty]
    public string ModVersion { get; set; } =  "1.0.0";

    // TODO: Everything
    // Index of all the files in the project
    // Changes maybe stored in their own files in subfolders?

    public FrostyProject()
    {
        FormatVersion = 1;
    }

    public static FrostyProject? Load(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        string jsonString = File.ReadAllText(path);
        FrostyProjectBase? @base = JsonConvert.DeserializeObject<FrostyProjectBase>(jsonString);
        if (@base is null)
        {
            return null;
        }

        return @base.FormatVersion switch
        {
            1 => JsonConvert.DeserializeObject<FrostyProject>(jsonString),
            _ => null
        };
    }

    public static async Task<FrostyProject?> LoadAsync(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        string jsonString = await File.ReadAllTextAsync(path);
        FrostyProjectBase? @base = JsonConvert.DeserializeObject<FrostyProjectBase>(jsonString);
        if (@base is null)
        {
            return null;
        }

        return @base.FormatVersion switch
        {
            1 => JsonConvert.DeserializeObject<FrostyProject>(jsonString),
            _ => null
        };
    }

    public bool SaveProject()
    {
        if (ProjectPath == string.Empty || ProjectDirectory is null || !Directory.Exists(ProjectDirectory))
        {
            return false;
        }

        string tmpName = "." + Path.GetFileName(ProjectPath) + ".tmp";
        string tmpPath = Path.Combine(ProjectDirectory, tmpName);
        using FileStream stream = File.Create(tmpPath);
        new StreamWriter(stream).Write(JsonConvert.SerializeObject(this));

        File.Replace(tmpPath, ProjectPath, null);

        return true;
    }
}