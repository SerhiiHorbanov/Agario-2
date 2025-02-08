using SFML.Audio;

namespace MyEngine.SoundSystem;

public class SoundLibrary
{
    private static readonly Dictionary<string, SoundBuffer> Sounds = new();
    private static readonly Dictionary<string, string> MusicNameToPath = new();
    
    public static void LoadAndStoreSound(string path, string name)
        => Sounds.Add(name, new(path));

    public static SoundBuffer GetSound(string name)
        => Sounds[name];

    public static void StoreMusic(string path, string name)
        => MusicNameToPath.Add(name, path);

    public static string GetMusicPath(string name)
        => MusicNameToPath[name];
}