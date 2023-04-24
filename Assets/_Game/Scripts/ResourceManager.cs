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

        if (GameDataManager.Instance.GetGameData().WeaponSO == null && WeaponListSO.WeaponSOList.Count > 0)
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
                return SkinListSO.SetSkinSOList;
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
        return GameDataManager.Instance.GetGameData().UnlockedSkinSOList.Contains(skinSO);
    }

    public bool IsSkinSelected(SkinSO skinSO)
    {
        return skinSO == GameDataManager.Instance.GetGameData().HatSkinSO ||
            skinSO == GameDataManager.Instance.GetGameData().PantSkinSO ||
            skinSO == GameDataManager.Instance.GetGameData().ShieldSkinSO ||
            skinSO == GameDataManager.Instance.GetGameData().FullSetSkinSO;
    }

    public bool TryBuySkin(SkinSO skinSO)
    {
        if (GameDataManager.Instance.GetGameData().GoldAmount < skinSO.Cost)
        {
            return false;
        }

        ReduceGoldAmount(skinSO.Cost);

        GameDataManager.Instance.GetGameData().UnlockedSkinSOList.Add(skinSO);

        GameDataManager.Instance.WriteFile();

        return true;
    }

    public void TrySkin(SkinSO skinSO)
    {
        Player.Instance.SetPlayerSkin(skinSO, false);
    }

    public void SelectSkin(SkinSO skinSO)
    {
        GameDataManager.Instance.GetGameData().SetSkinSOBySkinType(skinSO.SkinType, skinSO);

        GameDataManager.Instance.WriteFile();

        Player.Instance.SetPlayerSkin(skinSO);
    }

    public void UnselectSkin(SkinSO skinSO)
    {
        GameDataManager.Instance.GetGameData().SetSkinSOBySkinType(skinSO.SkinType, null);

        GameDataManager.Instance.WriteFile();

        Player.Instance.SetPlayerSkin(null);
    }

    public WeaponSO GetRandomWeaponSO()
    {
        int randomIndex = UnityEngine.Random.Range(0, WeaponListSO.WeaponSOList.Count);

        return WeaponListSO.WeaponSOList[randomIndex];
    }

    public bool IsWeaponUnlocked(WeaponSO weaponSO)
    {
        return GameDataManager.Instance.GetGameData().UnlockedWeaponSOList.Contains(weaponSO);
    }

    public bool IsWeaponSelected(WeaponSO weaponSO)
    {
        return weaponSO == GameDataManager.Instance.GetGameData().WeaponSO;
    }

    public bool TryBuyWeapon(WeaponSO weaponSO)
    {
        if (GameDataManager.Instance.GetGameData().GoldAmount < weaponSO.Cost)
        {
            return false;
        }

        ReduceGoldAmount(weaponSO.Cost);

        GameDataManager.Instance.GetGameData().UnlockedWeaponSOList.Add(weaponSO);

        GameDataManager.Instance.WriteFile();

        return true;
    }

    public void ChangeWeapon(WeaponSO weaponSO)
    {
        GameDataManager.Instance.GetGameData().WeaponSO = weaponSO;

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

    private void Bot_OnAnyBotDeath(object sender, EventArgs args)
    {
        Bot bot = sender as Bot;

        remainingObjectColorList.Add(bot.GetObjectColorSO());
        remainingNameListSO.Add(bot.GetName());
    }
}
