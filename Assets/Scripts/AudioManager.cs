using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public List<Sound> GameSounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound sound in GameSounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start()
    {
        PlaySound("SpaceWind");
    }

    public void PlaySound(string name)
    {
        Sound SoundToPlay = GameSounds.Find(SoundElement => SoundElement.name == name);
        
        if(SoundToPlay != null)
        {
            SoundToPlay.source.Play();
        }

        else
        {
            Debug.LogWarning("Sound " + name + " not found");
        }
    }
}
