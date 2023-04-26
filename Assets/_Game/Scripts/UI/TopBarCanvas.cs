using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarCanvas : UICanvas
{
    private const string UI_OPEN = "Open";
    private const string UI_CLOSE = "Close";

    [SerializeField]
    private TextMeshProUGUI survivalText;
    [SerializeField]
    private Button settingsButton;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        settingsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            GameManager.Instance.PauseGame();

            UIManager.Instance.CloseUI<GameplayCanvas>();
            UIManager.Instance.OpenUI<SettingsCanvas>();

            CloseDirectly();
        });
    }

    public override void Open()
    {
        base.Open();

        animator.SetTrigger(UI_OPEN);
    }

    public override void CloseDirectly()
    {
        animator.SetTrigger(UI_CLOSE);

        StartCoroutine(Utilities.DelayActionRealtimeCoroutine(0.15f, () =>
        {
            base.CloseDirectly();
        }));
    }

    public void SetSurvivalText(int survivalCount)
    {
        survivalText.text = $"Alive: {survivalCount}";
    }
}
