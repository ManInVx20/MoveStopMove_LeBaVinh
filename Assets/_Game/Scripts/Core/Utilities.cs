using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector3 GetRandomHorizontalDirection()
    {
        float randomX = Random.Range(-1.0f, 1.0f);
        float randomZ = Random.Range(-1.0f, 1.0f);

        return new Vector3(randomX, 0.0f, randomZ).normalized;
    }
}
