using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource musicSource, effectSource;
    
    protected override bool DestroyOnLoad => false;
    
    
    public static void PlayEffect(AudioClip clip, float duration = 1f)
    {
        Instance.effectSource.PlayOneShot(clip, duration);
    }
    
    public static void PlayMusic(AudioClip clip, bool isLoop = false)
    {
        Instance.musicSource.clip = clip;
        Instance.musicSource.loop = isLoop;
        Instance.musicSource.Play();
    }
    
    public static void StopMusic()
    {
        Instance.musicSource.Stop();
    }
    
    
}
