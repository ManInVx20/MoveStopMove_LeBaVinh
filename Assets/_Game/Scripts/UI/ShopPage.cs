using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPage : MonoBehaviour
{
    [SerializeField]
    private ShopSkin skinPrefab;
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

    private List<SkinSetSO> skinSetSOList;
    private List<ShopSkin> shopSkinList;
    private ShopSkin selectedShopSkin;

    private void Awake()
    {
        unlockButton.onClick.AddListener(() =>
        {
            if (ResourceManager.Instance.TryBuySkinSet(selectedShopSkin.GetSkinSetSO()))
            {
                UpdateButtons();

                selectedShopSkin.HideLockedGameObject();
            }
        });
        selectButton.onClick.AddListener(() =>
        {
            ResourceManager.Instance.ChangeSkinSet(selectedShopSkin.GetSkinSetSO());

            UpdateButtons();
        });
        unselectButton.onClick.AddListener(() =>
        {
            ResourceManager.Instance.ChangeSkinSet(null);

            UpdateButtons();
        });
    }

    private void Start()
    {
        skinSetSOList = ResourceManager.Instance.SkinSetListSO.SkinSetSOList;
        shopSkinList = new List<ShopSkin>();

        CreateAllSkins();

        if (shopSkinList != null && shopSkinList.Count > 0)
        {
            OnSkinSelected(shopSkinList[0]);
        }
    }

    public void OnSkinSelected(ShopSkin skin)
    {
        selectedShopSkin = skin;

        ResetSkins();

        skin.Activate();

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
        for (int i = 0; i < skinSetSOList.Count; i++)
        {
            ShopSkin skin = Instantiate(skinPrefab, contentTransform);
            skin.Initialize(this, skinSetSOList[i]);

            shopSkinList.Add(skin);
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
