using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Knife = 0,
    Hammer = 1,
    Boomerang = 2,
}

[CreateAssetMenu]
public class WeaponSO : ScriptableObject
{
    public WeaponType WeaponType;
    public string Name;
    public int Cost;
    public Sprite Sprite;
    public GameObject Prefab;
}
