using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyCanvas : UICanvas
{
    private const string UI_OPEN = "Open";
    private const string UI_CLOSE = "Close";

    [SerializeField]
    private TextMeshProUGUI goldText;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ResourceManager.Instance.OnGoldAmountChanged += ResourceManager_OnGoldAmountChanged;
    }

    public override void Setup()
    {
        base.Setup();

        goldText.text = ResourceManager.Instance.GetGoldAmount().ToString();
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

    private void ResourceManager_OnGoldAmountChanged(object sender, System.EventArgs args)
    {
        goldText.text = ResourceManager.Instance.GetGoldAmount().ToString();
    }
}
