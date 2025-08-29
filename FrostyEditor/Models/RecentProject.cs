using System;
using System.IO;

namespace FrostyEditor.Models;

public class RecentProject
{
    public string Name { get; }
    public string FullPath { get; }
    public DateTime LastOpened { get; }

    public RecentProject(string path, DateTime lastOpened)
    {
        Name = Path.GetFileNameWithoutExtension(path);
        FullPath = path;
        LastOpened = lastOpened;
    }
}