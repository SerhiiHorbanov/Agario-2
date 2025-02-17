namespace MyEngine.ConfigSystem;

public static class FilePathsLibrary
{
    private static Dictionary<string, string> FilePaths = new();
    
    public static string GetPath(string name)
        => FilePaths[name];

    public static void LoadAndStorePathsFromFile(string filePath)
        => ConfigLoader.LoadIntoDictionary(filePath, FilePaths);
}