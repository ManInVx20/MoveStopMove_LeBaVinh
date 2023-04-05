using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer bodySkinnedMeshRenderer;
    [SerializeField]
    private SkinnedMeshRenderer pantSkinnedMeshRenderer;
    [SerializeField]
    private Transform hatTransform;
    [SerializeField]
    private Transform topHatTransform;
    [SerializeField]
    private Transform wingTransform;
    [SerializeField]
    private Transform tailTransform;

    private SkinSetSO skinSetSO;

    public void SetBodyMaterial(Material material)
    {
        bodySkinnedMeshRenderer.material = material;
    }

    public void SetSkinSet(SkinSetSO skinSetSO)
    {
        this.skinSetSO = skinSetSO;

        if (skinSetSO.BodyMaterial != null)
        {
            bodySkinnedMeshRenderer.material = skinSetSO.BodyMaterial;
        }
        if (skinSetSO.PantMaterial != null)
        {
            pantSkinnedMeshRenderer.material = skinSetSO.PantMaterial;
            pantSkinnedMeshRenderer.gameObject.SetActive(true);
        }
        if (skinSetSO.HatPrefab != null)
        {
            Instantiate(skinSetSO.HatPrefab, hatTransform);
        }
        if (skinSetSO.TopHatPrefab != null)
        {
            Instantiate(skinSetSO.TopHatPrefab, topHatTransform);
        }
        if (skinSetSO.WingPrefab != null)
        {
            Instantiate(skinSetSO.WingPrefab, wingTransform);
        }
        if (skinSetSO.TailPrefab != null)
        {
            Instantiate(skinSetSO.TailPrefab, tailTransform);
        }
    }

    public void ResetSkinSet()
    {
        if (skinSetSO != null)
        {
            skinSetSO = null;

            pantSkinnedMeshRenderer.gameObject.SetActive(false);

            hatTransform.ClearChildren();
            topHatTransform.ClearChildren();
            wingTransform.ClearChildren();
            tailTransform.ClearChildren();

            //if (hatTransform.childCount > 0)
            //{
            //    Destroy(hatTransform.GetChild(0).gameObject);
            //}
            //if (topHatTransform.childCount > 0)
            //{
            //    Destroy(topHatTransform.GetChild(0).gameObject);
            //}
            //if (wingTransform.childCount > 0)
            //{
            //    Destroy(wingTransform.GetChild(0).gameObject);
            //}
            //if (tailTransform.childCount > 0)
            //{
            //    Destroy(tailTransform.GetChild(0).gameObject);
            //}
        }
    }
}
