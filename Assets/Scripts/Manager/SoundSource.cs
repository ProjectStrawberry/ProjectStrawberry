using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    private AudioSource _audiosource;

    public void Play(AudioClip clip, float soundEffectVolume, bool isMuted, float soundEffectPitchVariance)
    {
        if (_audiosource == null)
            _audiosource = GetComponent<AudioSource>();

        CancelInvoke();
        _audiosource.clip = clip;
        _audiosource.volume = soundEffectVolume;
        _audiosource.mute = isMuted;
        _audiosource.Play();
        _audiosource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);

        Invoke("Disable", clip.length + 2);
    }

    public void Disable()
    {
        _audiosource?.Stop();
        Destroy(this.gameObject);
    }
}
