using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkinSetSO : ScriptableObject
{
    public string Name;
    public int Cost;
    public Sprite Sprite;
    public Material BodyMaterial;
    public Material PantMaterial;
    public GameObject HatPrefab;
    public GameObject TopHatPrefab;
    public GameObject WingPrefab;
    public GameObject TailPrefab;
}
