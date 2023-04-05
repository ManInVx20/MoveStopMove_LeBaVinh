using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : UICanvas
{
    [SerializeField]
    private ButtonToggle soundButtonToggle;
    [SerializeField]
    private ButtonToggle vibrationButtonToggle;
    [SerializeField]
    private Button weaponButton;
    [SerializeField]
    private Button skinButton;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private TextMeshProUGUI zoneText;
    [SerializeField]
    private TMP_InputField playerNameInputField;

    private void Awake()
    {
        soundButtonToggle.OnValueChanged += SoundButtonToggle_OnValueChanged;
        vibrationButtonToggle.OnValueChanged += VibrationButtonToggle_OnValueChanged;

        weaponButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            GameManager.Instance.OpenWeaponShop();
        });
        skinButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            GameManager.Instance.OpenSkinShop();
        });
        playButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            GameManager.Instance.StartGame();
        });

        playerNameInputField.onEndEdit.AddListener((value) =>
        {
            string name = !string.IsNullOrEmpty(value) ? value : "Player";

            Player.Instance.SetPlayerName(name);

            playerNameInputField.text = name;
        });
    }

    private void SoundButtonToggle_OnValueChanged(object sender, ButtonToggle.OnValueChangedEventArgs args)
    {
        SoundManager.Instance.SetSound(args.Value);
    }

    private void VibrationButtonToggle_OnValueChanged(object sender, ButtonToggle.OnValueChangedEventArgs args)
    {
        SoundManager.Instance.SetVibration(args.Value);
    }

    public override void Setup()
    {
        base.Setup();

        goldText.text = ResourceManager.Instance.GetGoldAmount().ToString();
        zoneText.text = $"Zone: {LevelManager.Instance.GetLevelIndex()}";
        playerNameInputField.text = Player.Instance.GetName();

        soundButtonToggle.OnSwitchToggle(SoundManager.Instance.GetSound());
        vibrationButtonToggle.OnSwitchToggle(SoundManager.Instance.GetVibration());
    }
}
