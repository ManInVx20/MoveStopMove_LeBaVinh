using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetUI : MonoBehaviour
{
    [SerializeField]
    private Bot bot;
    [SerializeField]
    private Image targetImage;

    private void Start()
    {
        Bot.OnAnyBotDeath += Bot_OnBotDeath;
        bot.OnBotEnteredPlayerAttackRange += Bot_OnBotEnteredPlayerAttackRange;
        bot.OnBotExitedPlayerAttackRange += Bot_OnBotExitedPlayerAttackRange;

        Player.Instance.OnCharacterDeath += Player_OnCharacterDeath;

        Hide();
    }

    private void OnDestroy()
    {
        Player.Instance.OnCharacterDeath -= Player_OnCharacterDeath;
    }

    private void LateUpdate()
    {
        transform.forward = Vector3.down;
    }

    private void Bot_OnBotDeath(object sender, EventArgs args)
    {
        if ((Bot)sender == bot)
        {
            Hide();
        }
    }

    private void Bot_OnBotEnteredPlayerAttackRange(object sender, EventArgs args)
    {
        Show();
    }

    private void Bot_OnBotExitedPlayerAttackRange(object sender, EventArgs args)
    {
        Hide();
    }

    private void Player_OnCharacterDeath(object sender, EventArgs args)
    {
        Hide();
    }

    private void Show()
    {
        targetImage.gameObject.SetActive(true);
    }

    private void Hide()
    {
        targetImage.gameObject.SetActive(false);
    }
}
