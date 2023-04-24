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
    private float soundVolumeMultiplier;

    private void Awake()
    {
        cameraTrasform = Camera.main.transform;
    }

    private void Start()
    {
        SetSound(GameDataManager.Instance.GetGameData().Sound);
        SetVibration(GameDataManager.Instance.GetGameData().Vibration);
    }

    public bool GetSound()
    {
        return GameDataManager.Instance.GetGameData().Sound;
    }

    public void SetSound(bool value)
    {
        GameDataManager.Instance.GetGameData().Sound = value;

        if (GameDataManager.Instance.GetGameData().Sound)
        {
            soundVolumeMultiplier = 1.0f;
        }
        else
        {
            soundVolumeMultiplier = 0.0f;
        }

        GameDataManager.Instance.WriteFile();
    }

    public bool GetVibration()
    {
        return GameDataManager.Instance.GetGameData().Vibration;
    }

    public void SetVibration(bool value)
    {
        GameDataManager.Instance.GetGameData().Vibration = value;

        GameDataManager.Instance.WriteFile();
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

        AudioSource.PlayClipAtPoint(audioClip, position, volume * soundVolumeMultiplier);
    }

}
