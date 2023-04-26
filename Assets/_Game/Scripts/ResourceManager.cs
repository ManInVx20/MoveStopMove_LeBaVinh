using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    [field: SerializeField]
    public ObjectColorListSO ObjectColorListSO { get; private set; }
    [field: SerializeField]
    public NameListSO NameListSO { get; private set; }
    [field: SerializeField]
    public WeaponListSO WeaponListSO { get; private set; }
    [field: SerializeField]
    public SkinListSO SkinListSO { get; private set; }
    [field: SerializeField]
    public Level[] LevelPrefabs { get; private set; }
    [field: SerializeField]
    public GameObject PantPrefab { get; private set; }
    [field: SerializeField]
    public LevelUp LevelUpPrefab { get; private set; }

    public event EventHandler OnGoldAmountChanged;

    public CharacterInfoUIPool CharacterInfoUIPool { get; private set; }
    public TargetIndicatorUIPool TargetIndicatorUIPool { get; private set; }

    private List<ObjectColorSO> remainingObjectColorList;
    private List<string> remainingNameListSO;

    private void Awake()
    {
        CharacterInfoUIPool = GetComponent<CharacterInfoUIPool>();
        TargetIndicatorUIPool = GetComponent<TargetIndicatorUIPool>();

        ResetRemainingColorList();
        ResetRemainingNameList();
    }

    private void Start()
    {
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);

        Bot.OnAnyBotDeath += Bot_OnAnyBotDeath;

        if (GameDataManager.Instance.GetGameData().WeaponName == null && WeaponListSO.WeaponSOList.Count > 0)
        {
            WeaponSO weaponSO = WeaponListSO.WeaponSOList[0];

            if (TryBuyWeapon(weaponSO))
            {
                ChangeWeapon(weaponSO);
            }
        }
    }

    public void ResetState()
    {
        ResetRemainingColorList();
        ResetRemainingNameList();
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

    public string GetRandomName()
    {
        int randomIndex = UnityEngine.Random.Range(0, remainingObjectColorList.Count);
        string name = remainingNameListSO[randomIndex];

        remainingNameListSO.RemoveAt(randomIndex);

        return name;
    }

    public int GetGoldAmount()
    {
        return GameDataManager.Instance.GetGameData().GoldAmount;
    }

    public void IncreaseGoldAmount(int value)
    {
        GameDataManager.Instance.GetGameData().GoldAmount += value;

        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);

        GameDataManager.Instance.WriteFile();
    }

    public void ReduceGoldAmount(int value)
    {
        GameDataManager.Instance.GetGameData().GoldAmount -= value;

        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);

        GameDataManager.Instance.WriteFile();
    }

    public SkinSO GetSkinSOBySkinTypeAndSkinName(SkinType skinType, string skinName)
    {
        switch (skinType)
        {
            case SkinType.Hat:
                return GetSkinSOBySkinName(SkinListSO.HatSkinSOList, skinName);
            case SkinType.Pant:
                return GetSkinSOBySkinName(SkinListSO.PantSkinSOList, skinName);
            case SkinType.Accessary:
                return GetSkinSOBySkinName(SkinListSO.AccessarySkinSOList, skinName);
            case SkinType.FullSet:
                return GetSkinSOBySkinName(SkinListSO.FullSetSkinSOList, skinName);
            default:
                return null;
        }
    }

    public List<SkinSO> GetSkinSOListBySkinType(SkinType skinType)
    {
        switch (skinType)
        {
            case SkinType.Hat:
                return SkinListSO.HatSkinSOList;
            case SkinType.Pant:
                return SkinListSO.PantSkinSOList;
            case SkinType.Accessary:
                return SkinListSO.AccessarySkinSOList;
            case SkinType.FullSet:
                return SkinListSO.FullSetSkinSOList;
            default:
                return null;
        }
    }

    public SkinSO GetRandomSkinSOBySkinType(SkinType skinType)
    {
        List<SkinSO> skinSOList = GetSkinSOListBySkinType(skinType);

        int randomIndex = UnityEngine.Random.Range(0, skinSOList.Count);

        return skinSOList[randomIndex];
    }

    public bool IsSkinUnlocked(SkinSO skinSO)
    {
        return GameDataManager.Instance.GetGameData().UnlockedSkinNameList.Contains(skinSO.Name);
    }

    public bool IsSkinSelected(SkinSO skinSO)
    {
        return skinSO.Name.Equals(GameDataManager.Instance.GetGameData().HatSkinName) ||
            skinSO.Name.Equals(GameDataManager.Instance.GetGameData().PantSkinName) ||
            skinSO.Name.Equals(GameDataManager.Instance.GetGameData().ShieldSkinName) ||
            skinSO.Name.Equals(GameDataManager.Instance.GetGameData().FullSetSkinName);
    }

    public bool TryBuySkin(SkinSO skinSO)
    {
        if (GameDataManager.Instance.GetGameData().GoldAmount < skinSO.Cost)
        {
            return false;
        }

        ReduceGoldAmount(skinSO.Cost);

        GameDataManager.Instance.GetGameData().UnlockedSkinNameList.Add(skinSO.Name);

        GameDataManager.Instance.WriteFile();

        return true;
    }

    public void TrySkin(SkinSO skinSO)
    {
        Player.Instance.SetPlayerSkin(skinSO, false);
    }

    public void SelectSkin(SkinSO skinSO)
    {
        GameDataManager.Instance.GetGameData().SetSkinNameBySkinType(skinSO.SkinType, skinSO);

        GameDataManager.Instance.WriteFile();

        Player.Instance.SetPlayerSkin(skinSO);
    }

    public void UnselectSkin(SkinSO skinSO)
    {
        GameDataManager.Instance.GetGameData().SetSkinNameBySkinType(skinSO.SkinType, null);

        GameDataManager.Instance.WriteFile();

        Player.Instance.SetPlayerSkin(null);
    }

    public WeaponSO GetWeaponSOByName(string weaponName)
    {
        for (int i = 0; i < WeaponListSO.WeaponSOList.Count; i++)
        {
            if (WeaponListSO.WeaponSOList[i].Name.Equals(weaponName))
            {
                return WeaponListSO.WeaponSOList[i];
            }
        }

        return null;
    }

    public WeaponSO GetRandomWeaponSO()
    {
        int randomIndex = UnityEngine.Random.Range(0, WeaponListSO.WeaponSOList.Count);

        return WeaponListSO.WeaponSOList[randomIndex];
    }

    public bool IsWeaponUnlocked(WeaponSO weaponSO)
    {
        return GameDataManager.Instance.GetGameData().UnlockedWeaponNameList.Contains(weaponSO.Name);
    }

    public bool IsWeaponSelected(WeaponSO weaponSO)
    {
        return weaponSO.Name.Equals(GameDataManager.Instance.GetGameData().WeaponName);
    }

    public bool TryBuyWeapon(WeaponSO weaponSO)
    {
        if (GameDataManager.Instance.GetGameData().GoldAmount < weaponSO.Cost)
        {
            return false;
        }

        ReduceGoldAmount(weaponSO.Cost);

        GameDataManager.Instance.GetGameData().UnlockedWeaponNameList.Add(weaponSO.Name);

        GameDataManager.Instance.WriteFile();

        return true;
    }

    public void ChangeWeapon(WeaponSO weaponSO)
    {
        GameDataManager.Instance.GetGameData().WeaponName = weaponSO.Name;

        GameDataManager.Instance.WriteFile();

        Player.Instance.UpdateWeaponSO(weaponSO);
    }

    private void ResetRemainingColorList()
    {
        remainingObjectColorList = new List<ObjectColorSO>(ObjectColorListSO.ObjectColorSOList);
    }

    private void ResetRemainingNameList()
    {
        if (remainingNameListSO == null)
        {
            remainingNameListSO = new List<string>();
        }

        remainingNameListSO.Clear();

        for (int i = 0; i < NameListSO.NameList.Count; i++)
        {
            remainingNameListSO.Add(string.Copy(NameListSO.NameList[i]));
        }
    }

    private SkinSO GetSkinSOBySkinName(List<SkinSO> skinSOList, string skinName)
    {
        for (int i = 0; i < skinSOList.Count; i++)
        {
            if (skinSOList[i].Name.Equals(skinName))
            {
                return skinSOList[i];
            }
        }

        return null;
    }

    private void Bot_OnAnyBotDeath(object sender, EventArgs args)
    {
        Bot bot = sender as Bot;

        remainingObjectColorList.Add(bot.GetObjectColorSO());
        remainingNameListSO.Add(bot.GetName());
    }
}
