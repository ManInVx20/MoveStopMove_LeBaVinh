using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryCanvas : UICanvas
{
    [SerializeField]
    private TextMeshProUGUI rankText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private Button playAgainButton;

    private void Awake()
    {
        playAgainButton.onClick.AddListener(() =>
        {
            CloseDirectly();

            GameManager.Instance.WaitGame();
        });
    }

    public override void Setup()
    {
        base.Setup();

        Player player = Player.Instance;
        SetRankText(player.Rank, player.GetNormalColor());
        SetGoldText(player.Gold);
    }

    private void SetRankText(int rank, Color color)
    {
        rankText.text = $"#{rank}";
        rankText.color = color;
    }

    private void SetGoldText(int gold)
    {
        goldText.text = gold.ToString();
    }
}
