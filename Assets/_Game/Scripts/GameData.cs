using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int LevelIndex;
    public int GoldAmount;
    public string PlayerName;
    public string HatSkinName;
    public string PantSkinName;
    public string ShieldSkinName;
    public string FullSetSkinName;
    public string WeaponName;
    public List<string> UnlockedSkinNameList;
    public List<string> UnlockedWeaponNameList;
    public bool Sound;
    public bool Vibration;

    public GameData()
    {
        LevelIndex = 0;
        GoldAmount = 0;
        PlayerName = "You";
        HatSkinName = null;
        PantSkinName = null;
        ShieldSkinName = null;
        FullSetSkinName = null;
        WeaponName = null;
        UnlockedSkinNameList = new List<string>();
        UnlockedWeaponNameList = new List<string>();
        Sound = true;
        Vibration = true;
    }

    public void SetSkinNameBySkinType(SkinType skinType, SkinSO skinSO)
    {
        switch (skinType)
        {
            case SkinType.Hat:
                HatSkinName = skinSO?.Name;
                FullSetSkinName = null;
                break;
            case SkinType.Pant:
                PantSkinName = skinSO?.Name;
                FullSetSkinName = null;
                break;
            case SkinType.Accessary:
                ShieldSkinName = skinSO?.Name;
                FullSetSkinName = null;
                break;
            case SkinType.FullSet:
                FullSetSkinName = skinSO?.Name;
                HatSkinName = null;
                PantSkinName = null;
                ShieldSkinName = null;
                break;
        }
    }
}
