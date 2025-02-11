using MyEngine.SoundSystem;
using SFML.Audio;

namespace Agario_2;

public class AgarioSoundPlayer
{
    public static void PlayDashSound()
        => SoundManager.CreateSound("dash").WithRandomizedPitch(min: 0.7f, max: 1.3f).Play();

    public static void PlayMusic()
    {
        Music music = SoundManager.CreateMusic("invincible");
        
        music.Volume = 10;
        music.Loop = true;

        music.Play();
    }
}