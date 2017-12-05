using System;
using System.Collections;

using UnityEngine;

public static class AudioSourceExtensions {
    
    public static void TryPlaySFX(this AudioSource source, AudioClip clip, bool loop = false, float volume = 1f) {
        if (clip != null) {
            source.loop = loop;
            source.PlayOneShot(clip, volume);
        }
    }
    
    public static void TryPlayTheme(this AudioSource source, AudioClip clip) {
        if (clip != null) {
            source.loop = true;
            source.clip = clip;
            source.Play();
        }
    }

}
