using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : UICanvas
{
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
        weaponButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();
        });
        skinButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            GameManager.Instance.OpenShop();
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

    public override void Setup()
    {
        base.Setup();

        goldText.text = ResourceManager.Instance.GetGoldAmount().ToString();
        zoneText.text = $"Zone: {LevelManager.Instance.GetLevelIndex()}";
        playerNameInputField.text = Player.Instance.GetName();
    }
}
