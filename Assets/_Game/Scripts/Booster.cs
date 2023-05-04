using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : PoolableObject
{
    private void Update()
    {
        transform.Rotate(Vector3.up, -90.0f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Cache.TryGetCharacter(other, out Character character))
        {
            character.Boost();

            Despawn();
        }
    }

    private void Despawn()
    {
        ReturnToPool();
    }
}
