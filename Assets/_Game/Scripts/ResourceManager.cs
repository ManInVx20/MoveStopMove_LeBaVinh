using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    [field: SerializeField]
    public ObjectColorListSO ObjectColorListSO { get; private set; }
    [field: SerializeField]
    public WeaponListSO WeaponListSO { get; private set; }
    [field: SerializeField]
    public SkinSetListSO SkinSetListSO { get; private set; }
    [field: SerializeField]
    public Level[] LevelPrefabs { get; private set; }

    public event EventHandler OnGoldAmountChanged;

    public CharacterInfoUIPool CharacterInfoUIPool { get; private set; }
    public TargetIndicatorUIPool TargetIndicatorUIPool { get; private set; }

    private const string PLAYER_PREFS_GOLD = "Gold";
    private const string PLAYER_PREFS_SKIN_SET = "SkinSet";

    private List<ObjectColorSO> remainingObjectColorList;
    private int goldAmount;
    private string skinSetSelected;

    private void Awake()
    {
        ResetRemainingColorList();

        CharacterInfoUIPool = GetComponent<CharacterInfoUIPool>();
        TargetIndicatorUIPool = GetComponent<TargetIndicatorUIPool>();

        goldAmount = PlayerPrefs.GetInt(PLAYER_PREFS_GOLD, 0);
        skinSetSelected = PlayerPrefs.GetString(PLAYER_PREFS_SKIN_SET);
    }

    private void Start()
    {
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);

        Bot.OnAnyBotDeath += Bot_OnAnyBotDeath;
    }

    public void ResetRemainingColorList()
    {
        remainingObjectColorList = new List<ObjectColorSO>(ObjectColorListSO.ObjectColorSOList);
    }

    public ObjectColorSO GetFirstUnusedObjectColorSO()
    {
        ObjectColorSO objectColorSO = remainingObjectColorList[0];

        remainingObjectColorList.RemoveAt(0);

        return objectColorSO;
    }

    public ObjectColorSO GetRandomObjectColorSO()
    {
        int randomIndex = UnityEngine.Random.Range(0, remainingObjectColorList.Count);
        ObjectColorSO objectColorSO = remainingObjectColorList[randomIndex];

        remainingObjectColorList.RemoveAt(randomIndex);

        return objectColorSO;
    }

    public WeaponSO GetRandomWeaponSO()
    {
        int randomIndex = UnityEngine.Random.Range(0, WeaponListSO.WeaponSOList.Count);

        return WeaponListSO.WeaponSOList[randomIndex];
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }

    public void IncreaseGoldAmount(int value)
    {
        goldAmount += value;

        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);

        PlayerPrefs.SetInt(PLAYER_PREFS_GOLD, goldAmount);
    }

    public void ReduceGoldAmount(int value)
    {
        goldAmount -= value;

        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);

        PlayerPrefs.SetInt(PLAYER_PREFS_GOLD, goldAmount);
    }

    public SkinSetSO GetSelectedSkinSetSO()
    {
        for (int i = 0; i < SkinSetListSO.SkinSetSOList.Count; i++)
        {
            if (SkinSetListSO.SkinSetSOList[i].Name.Equals(skinSetSelected))
            {
                return SkinSetListSO.SkinSetSOList[i];
            }
        }

        return null;
    }

    public bool IsSkinSetUnlocked(SkinSetSO skinSetSO)
    {
        return PlayerPrefs.GetInt(skinSetSO.Name, 0) != 0;
    }

    public bool IsSkinSetSelected(SkinSetSO skinSetSO)
    {
        return !string.IsNullOrEmpty(skinSetSelected) && skinSetSelected.Equals(skinSetSO.Name);
    }

    public bool TryBuySkinSet(SkinSetSO skinSetSO)
    {
        if (goldAmount < skinSetSO.Cost)
        {
            return false;
        }

        ReduceGoldAmount(skinSetSO.Cost);

        PlayerPrefs.SetInt(skinSetSO.Name, 1);

        return true;
    }

    public void ChangeSkinSet(SkinSetSO skinSetSO)
    {
        skinSetSelected = skinSetSO?.Name;

        PlayerPrefs.SetString(PLAYER_PREFS_SKIN_SET, skinSetSelected);

        Player.Instance.UpdateSkinSetSO(skinSetSO);
    }

    private void Bot_OnAnyBotDeath(object sender, System.EventArgs args)
    {
        Bot bot = sender as Bot;

        remainingObjectColorList.Add(bot.GetObjectColorSO());
    }
}
