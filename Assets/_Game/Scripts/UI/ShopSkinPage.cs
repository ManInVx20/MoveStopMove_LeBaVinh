using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSkinPage : MonoBehaviour
{
    [SerializeField]
    private SkinType shopSkinType;
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

            if (ResourceManager.Instance.TryBuySkin(selectedShopSkin.GetSkinSO()))
            {
                UpdateButtons();

                selectedShopSkin.HideLockedGameObject();
            }
        });
        selectButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            ResourceManager.Instance.SelectSkin(selectedShopSkin.GetSkinSO());

            UpdateButtons();
        });
        unselectButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            ResourceManager.Instance.UnselectSkin(selectedShopSkin.GetSkinSO());

            UpdateButtons();
        });

        CreateAllSkins();
    }

    private void OnEnable()
    {
        ResetScrollview();

        if (shopSkinList != null && shopSkinList.Count > 0)
        {
            OnShopSkinSelected(shopSkinList[0]);
        }
    }

    public void OnShopSkinSelected(ShopSkin shopSkin)
    {
        selectedShopSkin = shopSkin;

        ResetSkins();

        shopSkin.Activate();

        ResourceManager.Instance.TrySkin(shopSkin.GetSkinSO());

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
        List<SkinSO> skinSOList = ResourceManager.Instance.GetSkinSOListBySkinType(shopSkinType);

        shopSkinList = new List<ShopSkin>();

        for (int i = 0; i < skinSOList.Count; i++)
        {
            ShopSkin shopSkin = Instantiate(shopSkinPrefab, contentTransform);
            shopSkin.Initialize(this, skinSOList[i]);

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
        if (ResourceManager.Instance.IsSkinUnlocked(selectedShopSkin.GetSkinSO()))
        {
            if (ResourceManager.Instance.IsSkinSelected(selectedShopSkin.GetSkinSO()))
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
            int cost = selectedShopSkin.GetSkinSO().Cost;
            costText.text = cost.ToString();

            if (cost > ResourceManager.Instance.GetGoldAmount())
            {
                unlockButton.interactable = false;
            }
            else
            {
                unlockButton.interactable = true;
            }

            unlockButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
            unselectButton.gameObject.SetActive(false);
        }
    }

    private void ResetScrollview()
    {
        Vector3 originLocalPosition = contentTransform.localPosition;
        originLocalPosition.x = 0.0f;
        contentTransform.localPosition = originLocalPosition;
    }
}
