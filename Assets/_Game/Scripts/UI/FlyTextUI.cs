using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlyTextUI : MonoBehaviour
{
    public enum Position
    {
        Left = 0,
        Right = 1,
    }
    
    [SerializeField]
    private TextMeshProUGUI flyText;

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
                Despawn();
            }
        }
    }

    public void Initialize(string content, Position position)
    {
        Vector3 newPosition = transform.localPosition; 
        switch (position)
        {
            case Position.Left:
                newPosition.x = -80.0f;
                break;
            case Position.Right:
                newPosition.x = 80.0f;
                break;
        }
        transform.localPosition = newPosition;

        flyText.text = content;

        startTimer = true;
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}
