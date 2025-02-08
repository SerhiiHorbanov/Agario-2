using MyEngine.Utils;
using SFML.Audio;

namespace MyEngine.SoundSystem;

public static class SoundManager
{
    public static List<Sound> PlayingSounds = new();
    public static List<Music> PlayingMusic = new();

    public static Music PlayMusic(string name)
    {
        Music result = new(SoundLibrary.GetMusicPath(name));
        PlayingMusic.Add(result);
        result.Play();
        
        return result;
    }
    
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
            if (PlayingSounds[i].ShouldBeRemoved())
            {
                PlayingSounds.SwapRemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < PlayingMusic.Count; i++)
        {
            if (PlayingMusic[i].ShouldBeRemoved())
            {
                PlayingSounds.SwapRemoveAt(i);
                i--;
            }
        }
    }

    private static bool ShouldBeRemoved(this Sound sound)
        => sound.Status == SoundStatus.Stopped;
    private static bool ShouldBeRemoved(this Music music)
        => music.Status == SoundStatus.Stopped;
}