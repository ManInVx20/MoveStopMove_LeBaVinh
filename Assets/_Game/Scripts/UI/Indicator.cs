using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    private const float OFFSET_X = 70.0f;
    private const float OFFSET_Y = 70.0f;

    [SerializeField]
    private Transform arrowTransform;
    [SerializeField]
    private Image arrowImage;
    [SerializeField]
    private Image levelBackgroundImage;
    [SerializeField]
    private TextMeshProUGUI levelText;

    public void SetScreenPosition(Vector3 position)
    {
        if (position.x < OFFSET_X)
        {
            position.x = OFFSET_X;
        } 
        else if (position.x > Screen.width - OFFSET_X)
        {
            position.x = Screen.width - OFFSET_X;
        }

        if (position.y < OFFSET_Y)
        {
            position.y = OFFSET_Y;
        }
        else if (position.y > Screen.height - OFFSET_Y)
        {
            position.y = Screen.height - OFFSET_Y;
        }

        transform.position = position;
    }

    public void SetArrowRotation(float angle)
    {
        arrowTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    public void SetColor(Color color)
    {
        arrowImage.color = color;
        levelBackgroundImage.color = color;
    }

    public void SetLevelText(int level)
    {
        levelText.text = level.ToString();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
