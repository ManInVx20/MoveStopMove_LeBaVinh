using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    private string saveFilePath;
    private GameData gameData = new GameData();

    private void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/data.json";

        ReadFile();
    }

    public GameData GetGameData()
    {
        return gameData;
    }

    public void ReadFile()
    {
        if (File.Exists(saveFilePath))
        {
            string fileContents = File.ReadAllText(saveFilePath);

            gameData = JsonUtility.FromJson<GameData>(fileContents);
        }
    }

    public void WriteFile()
    {
        string jsonString = JsonUtility.ToJson(gameData, true);

        File.WriteAllText(saveFilePath, jsonString);
    }
}
