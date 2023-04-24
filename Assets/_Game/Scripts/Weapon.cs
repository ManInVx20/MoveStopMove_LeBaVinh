using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject visualGameObject;

    private BulletPool bulletPool;
    private Coroutine hideShowVisualCoroutine;

    private void Awake()
    {
        bulletPool = GetComponent<BulletPool>();
    }

    public void PrepareToFire()
    {
        if (hideShowVisualCoroutine != null)
        {
            StopCoroutine(hideShowVisualCoroutine);
        }

        ShowVisual();
    }

    public void FireBullet(Transform parent, Character owner)
    {
        hideShowVisualCoroutine = StartCoroutine(HideShowVisualCoroutine(0.5f));

        Bullet bullet = bulletPool.GetPrefabInstance();
        bullet.transform.SetParent(parent, false);
        bullet.Fly(owner);
    }

    public void Despawn()
    {
        bulletPool.DestroyAllInstances();

        Destroy(gameObject);
    }

    private IEnumerator HideShowVisualCoroutine(float delayTime)
    {
        HideVisual();

        yield return new WaitForSeconds(delayTime);

        ShowVisual();
    }

    private void ShowVisual()
    {
        visualGameObject.SetActive(true);
    }

    private void HideVisual()
    {
        visualGameObject.SetActive(false);
    }
}
