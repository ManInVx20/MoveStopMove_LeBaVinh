using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int LevelIndex;
    public int GoldAmount;
    public string PlayerName;
    public SkinSO HatSkinSO;
    public SkinSO PantSkinSO;
    public SkinSO ShieldSkinSO;
    public SkinSO FullSetSkinSO;
    public WeaponSO WeaponSO;
    public List<SkinSO> UnlockedSkinSOList;
    public List<WeaponSO> UnlockedWeaponSOList;
    public bool Sound;
    public bool Vibration;

    public GameData()
    {
        LevelIndex = 0;
        GoldAmount = 0;
        PlayerName = "You";
        HatSkinSO = null;
        PantSkinSO = null;
        ShieldSkinSO = null;
        FullSetSkinSO = null;
        WeaponSO = null;
        UnlockedSkinSOList = new List<SkinSO>();
        UnlockedWeaponSOList = new List<WeaponSO>();
        Sound = true;
        Vibration = true;
    }

    public SkinSO GetSkinSOBySkinType(SkinType skinType)
    {
        switch (skinType)
        {
            case SkinType.Hat:
                return HatSkinSO;
            case SkinType.Pant:
                return PantSkinSO;
            case SkinType.Accessary:
                return ShieldSkinSO;
            case SkinType.FullSet:
                return FullSetSkinSO;
            default:
                return null;
        }
    }

    public void SetSkinSOBySkinType(SkinType skinType, SkinSO skinSO)
    {
        switch (skinType)
        {
            case SkinType.Hat:
                HatSkinSO = skinSO;
                FullSetSkinSO = null;
                break;
            case SkinType.Pant:
                PantSkinSO = skinSO;
                FullSetSkinSO = null;
                break;
            case SkinType.Accessary:
                ShieldSkinSO = skinSO;
                FullSetSkinSO = null;
                break;
            case SkinType.FullSet:
                FullSetSkinSO = skinSO;
                HatSkinSO = null;
                PantSkinSO = null;
                ShieldSkinSO = null;
                break;
        }
    }
}
