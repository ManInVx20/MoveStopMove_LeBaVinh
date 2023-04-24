using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSkin : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image skinImage;
    [SerializeField]
    private Transform skinHolderTransform;
    [SerializeField]
    private GameObject selectedGameObject;
    [SerializeField]
    private GameObject lockedGameObject;

    private ShopSkinPage skinPage;
    private SkinSO skinSO;

    private void Awake()
    {
        Deactivate();
    }

    public void Initialize(ShopSkinPage page, SkinSO skinSO)
    {
        this.skinPage = page;
        this.skinSO = skinSO;

        if (skinSO.Sprite != null)
        {
            skinImage.sprite = this.skinSO.Sprite;
        }
        else
        {
            skinImage.gameObject.SetActive(false);

            GameObject skinGameObject = new GameObject();

            switch (skinSO.SkinType)
            {
                case SkinType.Hat:
                    skinGameObject = Instantiate(skinSO.HatPrefab, skinHolderTransform);
                    break;
                case SkinType.Pant:
                    skinGameObject = Instantiate(ResourceManager.Instance.PantPrefab, skinHolderTransform);
                    skinGameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = skinSO.PantMaterial;
                    break;
                case SkinType.Accessary:
                    skinGameObject = Instantiate(skinSO.HatPrefab, skinHolderTransform);
                    break;
                case SkinType.FullSet:
                    skinGameObject = Instantiate(skinSO.HatPrefab, skinHolderTransform);
                    break;
            }

            skinGameObject.transform.SetLayer(LayerMask.NameToLayer("UI"), true);
        }

        if (ResourceManager.Instance.IsSkinUnlocked(skinSO))
        {
            HideLockedGameObject();
        }
    }

    public SkinSO GetSkinSO()
    {
        return skinSO;
    }

    public void HideLockedGameObject()
    {
        lockedGameObject.SetActive(false);
    }

    public void Activate()
    {
        selectedGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        selectedGameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skinPage.OnShopSkinSelected(this);
    }
}
