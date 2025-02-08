using MyEngine.Utils;
using SFML.Audio;

namespace MyEngine.SoundSystem;

public static class SoundManager
{
    public static List<Sound> PlayingSounds = new();
    
    public static void PlaySound(string name)
    {
        SoundBuffer buffer = SoundLibrary.GetSound(name);
        Sound sound = new Sound(buffer);
        
        sound.Play();
        
        PlayingSounds.Add(sound);
    }

    public static void UpdatePlayingSounds()
    {
        for (int i = 0; i < PlayingSounds.Count; i++)
        {
            if (PlayingSounds[0].ShouldBeRemoved())
            {
                PlayingSounds.SwapRemoveAt(i);
                i--;
            }
        }
    }

    private static bool ShouldBeRemoved(this Sound sound)
        => sound.Status == SoundStatus.Stopped;
}