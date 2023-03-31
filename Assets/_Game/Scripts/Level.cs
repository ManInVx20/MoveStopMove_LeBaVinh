using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [SerializeField]
    private NavMeshData navMeshData;
    [SerializeField]
    private BotSpawner botSpawner;

    private NavMeshDataInstance navMeshDataInstance;

    private void Awake()
    {
        navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);
    }

    private void OnDestroy()
    {
        NavMesh.RemoveNavMeshData(navMeshDataInstance);
    }

    public BotSpawner GetBotSpawner()
    {
        return botSpawner;
    }

    public void Despawn()
    {
        botSpawner.Despawn();

        Destroy(gameObject);
    }
}
