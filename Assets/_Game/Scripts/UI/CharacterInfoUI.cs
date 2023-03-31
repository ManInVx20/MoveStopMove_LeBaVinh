using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : PoolableObject
{
    [SerializeField]
    private Image levelBackgroundImage;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI levelText;

    private RectTransform rectTransform;
    private Camera cam;
    private Character owner;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (owner == null)
        {
            return;
        }

        Vector3 targetWorldPosition = owner.transform.position + Vector3.up * 3.0f * owner.GetMultiplier();
        rectTransform.position = cam.WorldToScreenPoint(targetWorldPosition);
    }

    public void Initialize(Character owner, Color color)
    {
        transform.SetParent(UIManager.Instance.GetUI<GameplayCanvas>().transform);

        this.owner = owner;
        this.owner.OnCharacterInfoChanged += Character_OnCharacterInfoChanged;

        ChangeColor(color);
    }

    public void ChangeColor(Color color)
    {
        nameText.color = color;
        levelBackgroundImage.color = color;
    }

    public void Despawn()
    {
        if (owner is Player)
        {
            Destroy(gameObject);
        }
        else
        {
            ReturnToPool();
        }

        owner = null;
    }

    private void Character_OnCharacterInfoChanged(object sender, Character.OnCharacterInfoChangedArgs args)
    {
        nameText.text = args.Name;
        levelText.text = args.Level.ToString();
    }
}
