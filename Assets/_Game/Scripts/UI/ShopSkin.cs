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
    private GameObject selectedGameObject;
    [SerializeField]
    private GameObject lockedGameObject;

    private ShopSkinPage skinPage;
    private SkinSetSO skinSetSO;

    private void Awake()
    {
        Deactivate();
    }

    public void Initialize(ShopSkinPage page, SkinSetSO skinSetSO)
    {
        this.skinPage = page;
        this.skinSetSO = skinSetSO;

        skinImage.sprite = this.skinSetSO.Sprite;

        if (ResourceManager.Instance.IsSkinSetUnlocked(skinSetSO))
        {
            HideLockedGameObject();
        }
    }

    public SkinSetSO GetSkinSetSO()
    {
        return skinSetSO;
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
