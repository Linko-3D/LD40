using UnityEngine;

public static class AudioSourceExtensions {
    
    public static void TryPlaySFX(this AudioSource source, AudioClip clip, float volume = 1f) {
        if (clip != null) {
            source.PlayOneShot(clip, volume);
        }
    }

    public static void TryPlayTheme(this AudioSource source, AudioClip clip) {
        if (clip != null) {
            source.clip = clip;
            source.Play();
        }
    }
}
