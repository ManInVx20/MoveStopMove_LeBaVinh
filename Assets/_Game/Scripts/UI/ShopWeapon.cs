using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopWeapon : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI weaponNameText;
    [SerializeField]
    private GameObject lockedGameObject;
    [SerializeField]
    private Transform weaponHolderTransform;

    private GameObject weapon;
    private WeaponSO weaponSO;

    private void OnEnable()
    {
        weaponHolderTransform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (weapon != null)
        {
            weaponHolderTransform.RotateAround(weaponHolderTransform.position, Vector3.up, 20.0f * Time.deltaTime);
        }
    }

    public void Initialize(ShopWeaponPage page, WeaponSO weaponSO)
    {
        this.weaponSO = weaponSO;

        weaponNameText.text = weaponSO.Name;

        weapon = Instantiate(weaponSO.Prefab, weaponHolderTransform);
        weapon.transform.SetLayer(LayerMask.NameToLayer("UI"), true);

        if (ResourceManager.Instance.IsWeaponUnlocked(weaponSO))
        {
            HideLockedGameObject();
        }
    }

    public WeaponSO GetWeaponSO()
    {
        return weaponSO;
    }

    public void HideLockedGameObject()
    {
        lockedGameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
