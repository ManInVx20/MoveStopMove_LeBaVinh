using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [SerializeField]
    private NavMeshData navMeshData;
    [SerializeField]
    private BotSpawner botSpawner;
    [SerializeField]
    private Color skyColor;

    private void Awake()
    {
        NavMesh.RemoveAllNavMeshData();
        
        NavMesh.AddNavMeshData(navMeshData);
    }

    public BotSpawner GetBotSpawner()
    {
        return botSpawner;
    }

    public Color GetSkyColor()
    {
        return skyColor;
    }

    public void Despawn()
    {
        botSpawner.Despawn();

        Destroy(gameObject);
    }
}
