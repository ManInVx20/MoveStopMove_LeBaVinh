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
    [SerializeField]
    private Transform shieldTransform;

    public void SetBodyMaterial(Material material)
    {
        bodySkinnedMeshRenderer.material = material;
    }

    public void SetPant(SkinSO pantSkinSO)
    {
        if (pantSkinSO == null)
        {
            return;
        }

        if (pantSkinSO.PantMaterial != null)
        {
            pantSkinnedMeshRenderer.material = pantSkinSO.PantMaterial;
            pantSkinnedMeshRenderer.gameObject.SetActive(true);
        }
    }

    public void SetHat(SkinSO hatSkinSO)
    {
        if (hatSkinSO == null)
        {
            return;
        }

        if (hatSkinSO.HatPrefab != null)
        {
            Instantiate(hatSkinSO.HatPrefab, hatTransform);
        }
    }

    public void SetShield(SkinSO shieldSkinSO)
    {
        if (shieldSkinSO == null)
        {
            return;
        }

        if (shieldSkinSO.ShieldPrefab != null)
        {
            Instantiate(shieldSkinSO.ShieldPrefab, shieldTransform);
        }
    }

    public void SetSet(SkinSO fullSetSkinSO)
    {
        if (fullSetSkinSO == null)
        {
            return;
        }

        if (fullSetSkinSO.BodyMaterial != null)
        {
            bodySkinnedMeshRenderer.material = fullSetSkinSO.BodyMaterial;
        }
        if (fullSetSkinSO.PantMaterial != null)
        {
            pantSkinnedMeshRenderer.material = fullSetSkinSO.PantMaterial;
            pantSkinnedMeshRenderer.gameObject.SetActive(true);
        }
        if (fullSetSkinSO.HatPrefab != null)
        {
            Instantiate(fullSetSkinSO.HatPrefab, hatTransform);
        }
        if (fullSetSkinSO.TopHatPrefab != null)
        {
            Instantiate(fullSetSkinSO.TopHatPrefab, topHatTransform);
        }
        if (fullSetSkinSO.WingPrefab != null)
        {
            Instantiate(fullSetSkinSO.WingPrefab, wingTransform);
        }
        if (fullSetSkinSO.TailPrefab != null)
        {
            Instantiate(fullSetSkinSO.TailPrefab, tailTransform);
        }
        if (fullSetSkinSO.ShieldPrefab != null)
        {
            Instantiate(fullSetSkinSO.ShieldPrefab, shieldTransform);
        }
    }

    public void ResetSkin()
    {
        pantSkinnedMeshRenderer.gameObject.SetActive(false);

        hatTransform.ClearChildren();
        topHatTransform.ClearChildren();
        wingTransform.ClearChildren();
        tailTransform.ClearChildren();
        shieldTransform.ClearChildren();
    }
}
