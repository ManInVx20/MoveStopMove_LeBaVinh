using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    public int BotRemainingCount => botRemainingCount;

    [SerializeField]
    private BotPool botPool;
    [SerializeField]
    private int firstSpawnCount = 10;
    [SerializeField]
    private int botMaxCount = 50;
    [SerializeField]
    private List<Transform> spawnPointList;

    private int botSpawnedCount;
    private int botRemainingCount;
    private int botDespawnedCountPerInterval;
    private float spawnTimer;
    private float spawnTime = 5.0f;

    private void Start()
    {
        botRemainingCount = botMaxCount;
        botSpawnedCount = 0;

        UIManager.Instance.GetUI<TopBarCanvas>().SetSurvivalText(botRemainingCount);
        UIManager.Instance.CloseUI<TopBarCanvas>();

        SpawnBots(firstSpawnCount, true);

        Bot.OnAnyBotDeath += Bot_OnAnyBotDeath;
        Bot.OnAnyBotDespawned += Bot_OnAnyBotDespawned;
    }

    private void Update()
    {
        if (botSpawnedCount < botMaxCount && botDespawnedCountPerInterval > 0)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnTime)
            {
                spawnTimer = 0.0f;

                int botToSpawnCount = botDespawnedCountPerInterval;

                SpawnBots(botToSpawnCount);

                botDespawnedCountPerInterval -= botToSpawnCount;
            }
        }
    }

    private void SpawnBots(int botToSpawnCount, bool firstWave = false)
    {
        List<Transform> safeSpawnPointList = GetSafeSpawnPointList();

        int maxIndex = Mathf.Min(botToSpawnCount, botRemainingCount);
        maxIndex = Mathf.Min(maxIndex, safeSpawnPointList.Count);

        for (int i = 0; i < maxIndex; i++)
        {
            if (botRemainingCount == 0 || botSpawnedCount == botMaxCount)
            {
                return;
            }

            Bot bot = botPool.GetPrefabInstance();

            if (!firstWave)
            {
                bot.ResetState();
            }

            int randomIndex = UnityEngine.Random.Range(0, safeSpawnPointList.Count);
            bot.GetAgent().Warp(safeSpawnPointList[randomIndex].position);

            safeSpawnPointList.RemoveAt(randomIndex);

            botSpawnedCount += 1;
        }
    }

    private void Bot_OnAnyBotDeath(object sender, EventArgs args)
    {
        botRemainingCount -= 1;

        UIManager.Instance.GetUI<TopBarCanvas>().SetSurvivalText(botRemainingCount);
    }

    private void Bot_OnAnyBotDespawned(object sender, EventArgs args)
    {
        botDespawnedCountPerInterval += 1;
    }

    private List<Transform> GetSafeSpawnPointList()
    {
        List<Transform> safeSpawnPointList = new List<Transform>();

        for (int i = 0; i < spawnPointList.Count; i++)
        {
            if (IsSafeSpawnPoint(spawnPointList[i].position))
            {
                safeSpawnPointList.Add(spawnPointList[i]);
            }
        }

        return safeSpawnPointList;
    }

    private bool IsSafeSpawnPoint(Vector3 position)
    {
        Collider[] colliderArray = Physics.OverlapSphere(position, 6.0f);

        for (int i = 0; i < colliderArray.Length; i++)
        {
            if (Cache.TryGetCharacter(colliderArray[i], out _))
            {
                return false;
            }
        }

        return true;
    }

    public void Despawn()
    {
        botPool.GetAllInstances().ForEach(instance => instance.DespawnWeapon());

        botPool.DestroyAllInstances();
    }
}
