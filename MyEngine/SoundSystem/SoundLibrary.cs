using SFML.Audio;

namespace MyEngine.SoundSystem;

public class SoundLibrary
{
    private static readonly Dictionary<string, SoundBuffer> Sounds = new();

    public static void LoadAndStoreSound(string path, string name)
        => Sounds.Add(name, new(path));

    public static SoundBuffer GetSound(string name)
        => Sounds[name];
}