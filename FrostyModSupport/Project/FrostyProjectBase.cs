using System.IO;
using Newtonsoft.Json;

namespace Frosty.ModSupport.Project;

public class FrostyProjectBase
{
    [JsonProperty]
    public uint FormatVersion { get; set; }
}