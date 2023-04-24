using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : UICanvas
{
    private const string UI_OPEN = "Open";
    private const string UI_CLOSE = "Close";

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
    private TextMeshProUGUI zoneText;
    [SerializeField]
    private TMP_InputField playerNameInputField;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

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
            string name = !string.IsNullOrEmpty(value) ? value : Player.PLAYER_NAME_DEFAULT;

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

        zoneText.text = $"Zone: {LevelManager.Instance.GetCurrentLevel()}";
        playerNameInputField.text = GameDataManager.Instance.GetGameData().PlayerName;

        soundButtonToggle.OnSwitchToggle(SoundManager.Instance.GetSound());
        vibrationButtonToggle.OnSwitchToggle(SoundManager.Instance.GetVibration());
    }

    public override void Open()
    {
        base.Open();

        animator.SetTrigger(UI_OPEN);
    }

    public override void CloseDirectly()
    {
        animator.SetTrigger(UI_CLOSE);

        StartCoroutine(Utilities.DelayActionCoroutine(0.15f, () =>
        {
            base.CloseDirectly();
        }));
    }
}
