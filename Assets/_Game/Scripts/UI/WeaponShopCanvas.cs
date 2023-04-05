using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopCanvas : UICanvas
{
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private Button closeButton;

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Utilities.GetCameraWithName("UICamera");

        closeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            GameManager.Instance.CloseWeaponShop();
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
