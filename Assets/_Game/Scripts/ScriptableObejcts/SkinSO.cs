using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkinType
{
    Hat = 0,
    Pant = 1,
    Accessary = 2,
    FullSet = 3,
}

[CreateAssetMenu]
public class SkinSO : ScriptableObject
{
    public SkinType SkinType;
    public string Name;
    public int Cost;
    public Sprite Sprite;
    public Material BodyMaterial;
    public Material PantMaterial;
    public GameObject HatPrefab;
    public GameObject TopHatPrefab;
    public GameObject WingPrefab;
    public GameObject TailPrefab;
    public GameObject ShieldPrefab;
}
