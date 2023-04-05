using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopWeaponPage : MonoBehaviour
{
    [SerializeField]
    private Button leftButton;
    [SerializeField]
    private Button rightButton;
    [SerializeField]
    private Transform shopWeaponHolderTransform;
    [SerializeField]
    private ShopWeapon shopWeaponPrefab;
    [SerializeField]
    private TextMeshProUGUI costText;
    [SerializeField]
    private Button unlockButton;
    [SerializeField]
    private Button selectButton;
    [SerializeField]
    private Button inUseButton;

    private List<ShopWeapon> shopWeaponList;
    private ShopWeapon selectedShopWeapon;
    private int shopWeaponIndex;

    private void Awake()
    {
        leftButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            shopWeaponIndex = (shopWeaponIndex - 1 + shopWeaponList.Count) % shopWeaponList.Count;

            OnShopWeponSelected(shopWeaponList[shopWeaponIndex]);
        });
        rightButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            shopWeaponIndex = (shopWeaponIndex + 1) % shopWeaponList.Count;

            OnShopWeponSelected(shopWeaponList[shopWeaponIndex]);
        });
        unlockButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            if (ResourceManager.Instance.TryBuyWeapon(selectedShopWeapon.GetWeaponSO()))
            {
                UpdateButtons();

                selectedShopWeapon.HideLockedGameObject();
            }

        });
        selectButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            ResourceManager.Instance.ChangeWeapon(selectedShopWeapon.GetWeaponSO());

            UpdateButtons();
        });
    }

    private void Start()
    {
        CreateAllWeapons();

        if (shopWeaponList.Count > 0)
        {
            shopWeaponIndex = 0;

            OnShopWeponSelected(shopWeaponList[shopWeaponIndex]);
        }
    }

    private void CreateAllWeapons()
    {
        List<WeaponSO> weaponSOList = ResourceManager.Instance.WeaponListSO.WeaponSOList;
        shopWeaponList = new List<ShopWeapon>();

        for (int i = 0; i < weaponSOList.Count; i++)
        {
            ShopWeapon shopWeapon = Instantiate(shopWeaponPrefab, shopWeaponHolderTransform);
            shopWeapon.Initialize(this, weaponSOList[i]);

            shopWeaponList.Add(shopWeapon);
        }
    }

    private void OnShopWeponSelected(ShopWeapon shopWeapon)
    {
        selectedShopWeapon = shopWeapon;

        ResetShopWeapons();

        shopWeapon.Show();

        UpdateButtons();
    }

    private void ResetShopWeapons()
    {
        for (int i = 0; i < shopWeaponList.Count; i++)
        {
            if (shopWeaponList[i] == selectedShopWeapon)
            {
                continue;
            }
            else
            {
                shopWeaponList[i].Hide();
            }
        }
    }

    private void UpdateButtons()
    {
        if (ResourceManager.Instance.IsWeaponUnlocked(selectedShopWeapon.GetWeaponSO()))
        {
            if (ResourceManager.Instance.IsWeaponSelected(selectedShopWeapon.GetWeaponSO()))
            {
                unlockButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(false);
                inUseButton.gameObject.SetActive(true);
            }
            else
            {
                unlockButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
                inUseButton.gameObject.SetActive(false);
            }
        }
        else
        {
            costText.text = selectedShopWeapon.GetWeaponSO().Cost.ToString();

            unlockButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
            inUseButton.gameObject.SetActive(false);
        }
    }
}
