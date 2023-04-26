using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelUpText;

    private float timer = 0.0f;
    private float timerMax = 0.5f;
    private bool startTimer = false;

    private void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Initialize(int levelUpValue)
    {
        levelUpText.text = "+" + levelUpValue;

        startTimer = true;
    }
}
