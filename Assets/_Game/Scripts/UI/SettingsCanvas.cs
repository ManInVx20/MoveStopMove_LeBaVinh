using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsCanvas : UICanvas
{
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Button continueButton;

    private void Awake()
    {
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
}
