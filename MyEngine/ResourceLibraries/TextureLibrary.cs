using SFML.Graphics;

namespace MyEngine.ResourceLibraries;

public static class TextureLibrary
{
    private static readonly Dictionary<string, Texture> Textures = new();
    
    public static void LoadAndStoreTexture(string path, string name)
        => Textures.Add(name, new(path));
    public static Texture GetTexture(string name)
        => Textures[name];
}