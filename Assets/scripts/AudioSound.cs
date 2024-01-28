using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioSound
{

    public string name;

    public AudioClip clip;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
    
}
