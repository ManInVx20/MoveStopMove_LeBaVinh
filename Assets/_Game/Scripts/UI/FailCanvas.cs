using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FailCanvas : UICanvas
{
    [SerializeField]
    private TextMeshProUGUI rankText;
    [SerializeField]
    private TextMeshProUGUI killerNameText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private Button playAgainButton;

    private void Awake()
    {
        playAgainButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClickSound();

            CloseDirectly();

            GameManager.Instance.WaitGame();
        });
    }

    public override void Setup()
    {
        base.Setup();

        Player player = Player.Instance;
        SetRankText(player.Rank, player.GetNormalColor());
        SetKillerNameText(player.Killer.GetName(), player.Killer.GetNormalColor());
        SetGoldText(player.Gold);
    }

    private void SetRankText(int rank, Color color)
    {
        rankText.text = $"#{rank}";
        rankText.color = color;
    }

    private void SetKillerNameText(string killerName, Color color)
    {
        killerNameText.text = killerName;
        killerNameText.color = color;
    }

    private void SetGoldText(int gold)
    {
        goldText.text = gold.ToString();
    }
}
