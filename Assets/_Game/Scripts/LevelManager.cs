using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private const string PLAYER_PREFS_LEVEL = "Level";

    private Level level;
    private int levelIndex;
    private Camera mainCamera;

    private void Awake()
    {
        levelIndex = PlayerPrefs.GetInt(PLAYER_PREFS_LEVEL, 1);
        mainCamera = Camera.main;
    }

    public void NextLevel()
    {
        if (levelIndex + 1 <= ResourceManager.Instance.LevelPrefabs.Length)
        {
            levelIndex += 1;

            PlayerPrefs.SetInt(PLAYER_PREFS_LEVEL, levelIndex);
        }
    }

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    public void LoadLevel()
    {
        if (level != null)
        {
            level.Despawn();
        }

        level = Instantiate(ResourceManager.Instance.LevelPrefabs[levelIndex - 1]);
        mainCamera.backgroundColor = level.GetSkyColor();
    }

    public BotSpawner GetBotSpawner()
    {
        return level?.GetBotSpawner();
    }
}
