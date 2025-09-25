namespace FrostyEditor.Services;

public interface IProjectService
{

    /// <summary>
    /// Create a project in a new folder
    /// </summary>
    /// <param name="modName">The Mod name</param>
    /// <param name="modVersion">The Mod version</param>
    /// <param name="createInFolder">The parent folder. A new subfolder will be created for the project</param>
    /// <param name="profileSlug">The profile instance slug to associate with the project</param>
    /// <returns>The project path if successful</returns>
    public string? CreateProject(string modName, string modVersion, string createInFolder, string profileSlug);

    /// <summary>
    /// Open a project in a new window. This should only be called from a View!
    /// </summary>
    /// <param name="projectPath">The path to the project</param>
    public void OpenProject(string projectPath);

}