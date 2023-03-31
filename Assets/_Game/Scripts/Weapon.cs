using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private BulletPool bulletPool;

    private void Awake()
    {
        bulletPool = GetComponent<BulletPool>();
    }

    public void FireBullet(Transform parent, Character owner)
    {
        Bullet bullet = bulletPool.GetPrefabInstance();
        bullet.transform.SetParent(parent, false);
        bullet.Fly(owner);
    }

    public void Despawn()
    {
        bulletPool.DestroyAllInstances();

        Destroy(gameObject);
    }
}
