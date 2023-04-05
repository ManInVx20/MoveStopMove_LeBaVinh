using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSkinPage : MonoBehaviour
{
    [SerializeField]
    private ShopSkin shopSkinPrefab;
    [SerializeField]
    private Transform contentTransform;
    [SerializeField]
    private TextMeshProUGUI costText;
    [SerializeField]
    private Button unlockButton;
    [SerializeField]
    private Button selectButton;
    [SerializeField]
    private Button unselectButton;

    private List<ShopSkin> shopSkinList;
    private ShopSkin selectedShopSkin;

    private void Awake()
    {
        unlockButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            if (ResourceManager.Instance.TryBuySkinSet(selectedShopSkin.GetSkinSetSO()))
            {
                UpdateButtons();

                selectedShopSkin.HideLockedGameObject();
            }
        });
        selectButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            ResourceManager.Instance.ChangeSkinSet(selectedShopSkin.GetSkinSetSO());

            UpdateButtons();
        });
        unselectButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            ResourceManager.Instance.ChangeSkinSet(null);

            UpdateButtons();
        });
    }

    private void Start()
    {
        CreateAllSkins();

        if (shopSkinList.Count > 0)
        {
            OnShopSkinSelected(shopSkinList[0]);
        }
    }

    public void OnShopSkinSelected(ShopSkin shopSkin)
    {
        selectedShopSkin = shopSkin;

        ResetSkins();

        shopSkin.Activate();

        UpdateButtons();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void CreateAllSkins()
    {
        List<SkinSetSO> skinSetSOList = ResourceManager.Instance.SkinSetListSO.SkinSetSOList;
        shopSkinList = new List<ShopSkin>();

        for (int i = 0; i < skinSetSOList.Count; i++)
        {
            ShopSkin shopSkin = Instantiate(shopSkinPrefab, contentTransform);
            shopSkin.Initialize(this, skinSetSOList[i]);

            shopSkinList.Add(shopSkin);
        }
    }

    private void ResetSkins()
    {
        for (int i = 0; i < shopSkinList.Count; i++)
        {
            if (shopSkinList[i] == selectedShopSkin)
            {
                continue;
            }
            else
            {
                shopSkinList[i].Deactivate();
            }
        }
    }

    private void UpdateButtons()
    {
        if (ResourceManager.Instance.IsSkinSetUnlocked(selectedShopSkin.GetSkinSetSO()))
        {
            if (ResourceManager.Instance.IsSkinSetSelected(selectedShopSkin.GetSkinSetSO()))
            {
                unlockButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(false);
                unselectButton.gameObject.SetActive(true);
            }
            else
            {
                unlockButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
                unselectButton.gameObject.SetActive(false);
            }
        }
        else
        {
            costText.text = selectedShopSkin.GetSkinSetSO().Cost.ToString();

            unlockButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
            unselectButton.gameObject.SetActive(false);
        }
    }
}
