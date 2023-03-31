using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackRangeUI : MonoBehaviour
{
    [SerializeField]
    private Character character;
    [SerializeField]
    private AttackRange attackRange;
    [SerializeField]
    private Image rangeImage;

    private void Start()
    {
        character.OnCharacterStarted += Character_OnCharacterStarted;
        character.OnCharacterDeath += Character_OnCharacterDeath;

        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void LateUpdate()
    {
        transform.forward = Vector3.down;
    }

    private void Character_OnCharacterStarted(object sender, EventArgs args)
    {
        Hide();
    }

    private void Character_OnCharacterDeath(object sender, EventArgs args)
    {
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs args)
    {
        if (GameManager.Instance.IsGameStarted())
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
