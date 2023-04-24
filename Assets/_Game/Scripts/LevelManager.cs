using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private Level level;
    private int levelIndex;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        levelIndex = GameDataManager.Instance.GetGameData().LevelIndex;
    }

    public void NextLevel()
    {
        if (levelIndex < ResourceManager.Instance.LevelPrefabs.Length)
        {
            GameDataManager.Instance.GetGameData().LevelIndex += 1;

            GameDataManager.Instance.WriteFile();
        }
    }

    public int GetCurrentLevel()
    {
        return levelIndex + 1;
    }

    public void LoadLevel()
    {
        if (level != null)
        {
            level.Despawn();
        }

        level = Instantiate(ResourceManager.Instance.LevelPrefabs[levelIndex]);
        mainCamera.backgroundColor = level.GetSkyColor();
    }

    public BotSpawner GetBotSpawner()
    {
        return level?.GetBotSpawner();
    }
}
