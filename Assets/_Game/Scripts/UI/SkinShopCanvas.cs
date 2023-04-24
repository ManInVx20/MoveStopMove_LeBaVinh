using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopCanvas : UICanvas
{
    private const string UI_OPEN = "Open";
    private const string UI_CLOSE = "Close";

    [SerializeField]
    private Button closeButton;

    private Canvas canvas;
    private Animator animator;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Utilities.GetCameraWithName("UICamera");

        animator = GetComponent<Animator>();

        closeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            GameManager.Instance.CloseSkinShop();
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

        StartCoroutine(Utilities.DelayActionCoroutine(0.15f, () =>
        {
            base.CloseDirectly();
        }));
    }
}
