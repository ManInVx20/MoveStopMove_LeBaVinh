using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPointList;

    private BoosterPool boosterPool;
    private float spawnTimer;
    private float spawnTimerMax = 10.0f;

    private void Awake()
    {
        boosterPool = GetComponent<BoosterPool>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameStarted())
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnTimerMax)
            {
                spawnTimer = 0.0f;

                SpawnBoosters();
            }
        }
    }

    private void SpawnBoosters()
    {
        int randomNumber = UnityEngine.Random.Range(0, spawnPointList.Count);
        if (UnityEngine.Random.Range(0, 2) > 0)
        {
            for (int i = 0; i < randomNumber; i++)
            {
                if (IsSafeSpawnPoint(spawnPointList[i]))
                {
                    SpawnBooster(spawnPointList[i]);
                }
            }
        }
        else
        {
            for (int i = spawnPointList.Count - 1; i > spawnPointList.Count - 1 - randomNumber; i--)
            {
                if (spawnPointList[i].childCount == 0)
                {
                    SpawnBooster(spawnPointList[i]);
                }
            }
        }
    }

    private bool IsSafeSpawnPoint(Transform point)
    {
        if (point.childCount > 0)
        {
            return false;
        }

        Collider[] colliderArray = Physics.OverlapSphere(point.position, 6.0f);

        for (int i = 0; i < colliderArray.Length; i++)
        {
            if (Cache.TryGetCharacter(colliderArray[i], out _))
            {
                return false;
            }
        }

        return true;
    }

    private void SpawnBooster(Transform parent)
    {
        Booster booster = boosterPool.GetPrefabInstance();
        booster.transform.SetParent(parent, false);
    }
}
