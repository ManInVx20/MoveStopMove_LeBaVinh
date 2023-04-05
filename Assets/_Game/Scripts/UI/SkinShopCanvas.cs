using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopCanvas : UICanvas
{
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            GameManager.Instance.CloseSkinShop();
        });
    }

    private void Start()
    {
        ResourceManager.Instance.OnGoldAmountChanged += ResourceManager_OnGoldAmountChanged;
    }

    private void ResourceManager_OnGoldAmountChanged(object sender, System.EventArgs args)
    {
        goldText.text = ResourceManager.Instance.GetGoldAmount().ToString();
    }

    public override void Setup()
    {
        base.Setup();

        goldText.text = ResourceManager.Instance.GetGoldAmount().ToString();
    }
}
