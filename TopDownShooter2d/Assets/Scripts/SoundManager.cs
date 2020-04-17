using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    public void PlayOneShot(string name)
    {
        Sound s=Array.Find(sounds, sound => sound.name == name);
        s.source.PlayOneShot(s.clip);
    }
}
