using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponSO : ScriptableObject
{
    public string Name;
    public int Cost;
    public Sprite Sprite;
    public GameObject Prefab;
}
