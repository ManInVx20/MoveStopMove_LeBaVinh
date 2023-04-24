using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBullet : Bullet
{
    private float surviveTimer;
    private float surviveTime = 0.8f;

    public override void PrepareToUse()
    {
        base.PrepareToUse();

        surviveTimer = 0.0f;
    }

    protected override void Execute()
    {
        base.Execute();

        surviveTimer += Time.deltaTime;
        if (surviveTimer >= surviveTime)
        {
            Despawn();
        }
        else
        {
            transform.Rotate(Vector3.up * -720.0f * Time.deltaTime);
        }
    }

    protected override void Despawn()
    {
        base.Despawn();
    }
}
