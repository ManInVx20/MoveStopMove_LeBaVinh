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
    private const string PLAYER_PREFS_WEAPON = "Weapon";

    private List<ObjectColorSO> remainingObjectColorList;
    private int goldAmount;
    private string skinSetSelected;
    private string weaponSelected;

    private void Awake()
    {
        ResetRemainingColorList();

        CharacterInfoUIPool = GetComponent<CharacterInfoUIPool>();
        TargetIndicatorUIPool = GetComponent<TargetIndicatorUIPool>();

        goldAmount = PlayerPrefs.GetInt(PLAYER_PREFS_GOLD, 0);
        skinSetSelected = PlayerPrefs.GetString(PLAYER_PREFS_SKIN_SET);
        weaponSelected = PlayerPrefs.GetString (PLAYER_PREFS_WEAPON);
    }

    private void Start()
    {
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);

        Bot.OnAnyBotDeath += Bot_OnAnyBotDeath;

        if (string.IsNullOrEmpty(weaponSelected) && WeaponListSO.WeaponSOList.Count > 0)
        {
            WeaponSO weaponSO = WeaponListSO.WeaponSOList[0];

            if (TryBuyWeapon(weaponSO))
            {
                ChangeWeapon(weaponSO);
            }
        }
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

    public SkinSetSO GetRandomSkinSetSO()
    {
        int randomIndex = UnityEngine.Random.Range(0, SkinSetListSO.SkinSetSOList.Count);

        return SkinSetListSO.SkinSetSOList[randomIndex];
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

    public WeaponSO GetSelectedWeaponSO()
    {
        for (int i = 0; i < WeaponListSO.WeaponSOList.Count; i++)
        {
            if (WeaponListSO.WeaponSOList[i].Name.Equals(weaponSelected))
            {
                return WeaponListSO.WeaponSOList[i];
            }
        }

        return null;
    }

    public bool IsWeaponUnlocked(WeaponSO weaponSO)
    {
        return PlayerPrefs.GetInt(weaponSO.Name, 0) != 0;
    }

    public bool IsWeaponSelected(WeaponSO weaponSO)
    {
        return !string.IsNullOrEmpty(weaponSelected) && weaponSelected.Equals(weaponSO.Name);
    }

    public bool TryBuyWeapon(WeaponSO weaponSO)
    {
        if (goldAmount < weaponSO.Cost)
        {
            return false;
        }

        ReduceGoldAmount(weaponSO.Cost);

        PlayerPrefs.SetInt(weaponSO.Name, 1);

        return true;
    }

    public void ChangeWeapon(WeaponSO weaponSO)
    {
        weaponSelected = weaponSO?.Name;

        PlayerPrefs.SetString(PLAYER_PREFS_WEAPON, weaponSelected);

        Player.Instance.UpdateWeaponSO(weaponSO);
    }

    private void Bot_OnAnyBotDeath(object sender, System.EventArgs args)
    {
        Bot bot = sender as Bot;

        remainingObjectColorList.Add(bot.GetObjectColorSO());
    }
}
