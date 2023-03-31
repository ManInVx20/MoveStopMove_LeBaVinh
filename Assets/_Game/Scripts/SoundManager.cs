using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioClip buttonClickAudioClip;
    [SerializeField]
    private AudioClip throwAudioClip;
    [SerializeField]
    private AudioClip hitAudioClip;
    [SerializeField]
    private AudioClip sizeUpAudioClip;
    [SerializeField]
    private AudioClip deathAudioClip;
    [SerializeField]
    private AudioClip failAudioClip;
    [SerializeField]
    private AudioClip victoryAudioClip;

    private Transform cameraTrasform;

    private void Awake()
    {
        cameraTrasform = Camera.main.transform;
    }

    public void PlayButtonClickSound()
    {
        PlaySound(buttonClickAudioClip, cameraTrasform.position, 0.5f);
    }

    public void PlayThrowSound(Vector3 position)
    {
        PlaySound(throwAudioClip, position, 0.5f);
    }

    public void PlayHitSound(Vector3 position)
    {
        PlaySound(hitAudioClip, position, 0.5f);
    }

    public void PlaySizeUpSound(Vector3 position)
    {
        PlaySound(sizeUpAudioClip, position, 0.5f);
    }

    public void PlayDeathSound(Vector3 position)
    {
        PlaySound(deathAudioClip, position, 0.5f);
    }

    public void PlayFailSound()
    {
        PlaySound(failAudioClip, cameraTrasform.position, 0.75f);
    }

    public void PlayVictorySound()
    {
        PlaySound(victoryAudioClip, cameraTrasform.position, 0.75f);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1.0f)
    {
        volume = Mathf.Clamp01(volume);

        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

}
