using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsCanvas : UICanvas
{
    [SerializeField]
    private SwitchToggle soundSwitchToggle;
    [SerializeField]
    private SwitchToggle vibrationSwitchToggle;
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Button continueButton;

    private void Awake()
    {
        soundSwitchToggle.OnValueChanged += SoundSwitchToggle_OnValueChanged;
        vibrationSwitchToggle.OnValueChanged += VibrationSwitchToggle_OnValueChanged;

        homeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            GameManager.Instance.ResumeGame();

            GameManager.Instance.WaitGame();

            CloseDirectly();
        });
        continueButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            GameManager.Instance.ResumeGame();

            UIManager.Instance.OpenUI<GameplayCanvas>();
            UIManager.Instance.OpenUI<ControlCanvas>();

            CloseDirectly();
        });
    }

    public override void Setup()
    {
        base.Setup();

        soundSwitchToggle.OnSwitchToggle(SoundManager.Instance.GetSound());
        vibrationSwitchToggle.OnSwitchToggle(SoundManager.Instance.GetVibration());
    }

    private void SoundSwitchToggle_OnValueChanged(object sender, SwitchToggle.OnValueChangedEventArgs args)
    {
        SoundManager.Instance.SetSound(args.Value);
    }

    private void VibrationSwitchToggle_OnValueChanged(object sender, SwitchToggle.OnValueChangedEventArgs args)
    {
        SoundManager.Instance.SetVibration(args.Value);
    }
}
