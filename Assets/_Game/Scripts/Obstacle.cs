using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material transparentMaterial;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        SetNormal();
    }

    public void SetNormal()
    {
        meshRenderer.material = normalMaterial;
    }

    public void SetTrasparent()
    {
        meshRenderer.material = transparentMaterial;
    }
}
