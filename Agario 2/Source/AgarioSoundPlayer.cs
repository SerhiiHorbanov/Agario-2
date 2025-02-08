using MyEngine.SoundSystem;
using SFML.Audio;

namespace Agario_2;

public class AgarioSoundPlayer
{
    public static void PlayDashSound()
        => SoundManager.PlaySound("dash");

    public static void PlayMusic()
    {
        Music music = SoundManager.PlayMusic("invincible");
        
        music.Volume = 10;
        music.Loop = true;
        
        Console.WriteLine("playing music");
    }
}