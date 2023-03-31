using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : UICanvas
{
    [SerializeField]
    private TextMeshProUGUI survivalText;
    [SerializeField]
    private Button settingsButton;

    private void Awake()
    {
        settingsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            GameManager.Instance.PauseGame();

            UIManager.Instance.OpenUI<SettingsCanvas>();

            UIManager.Instance.CloseUI<ControlCanvas>();
            
            CloseDirectly();
        });
    }

    public void SetSurvivalText(int survivalCount)
    {
        survivalText.text = $"Alive: {survivalCount}";
    }
}
