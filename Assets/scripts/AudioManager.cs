using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioSound[] sounds;
    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (AudioSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }

    }

    public void Stop(String name)
    {
        AudioSound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void Play(String name)
    {
        AudioSound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
