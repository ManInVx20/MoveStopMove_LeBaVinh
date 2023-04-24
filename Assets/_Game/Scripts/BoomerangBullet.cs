using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBullet : Bullet
{
    private float returnTimer;
    private float returnTime = 0.8f;

    public override void PrepareToUse()
    {
        base.PrepareToUse();

        returnTimer = 0.0f;
    }

    protected override void Execute()
    {
        base.Execute();

        transform.Rotate(Vector3.up * -720.0f * Time.deltaTime);

        returnTimer += Time.deltaTime;
        if (returnTimer >= returnTime)
        {
            if (GetOwner() == null)
            {
                Despawn();
            }

            Vector3 ownerPosition = GetOwner().transform.position + Vector3.up;
            Vector3 direction = (ownerPosition - transform.position).normalized;
            SetDirection(direction);
            SetSpeedMultiplier(1.5f);

            if (Vector3.Distance(transform.position, ownerPosition) < 0.5f)
            {
                Despawn();
            }
        }
    }

    protected override void Despawn()
    {
        base.Despawn();
    }
}
