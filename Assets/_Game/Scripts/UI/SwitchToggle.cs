using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField]
    private Image handleImage;
    [SerializeField]
    private Sprite onSprite;
    [SerializeField]
    private Sprite offSprite;

    private Toggle toggle;
    private Vector2 handlePosition;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = handleImage.rectTransform.anchoredPosition;
        toggle.onValueChanged.AddListener(OnSwitchToggle);
    }

    private void Start()
    {
        OnSwitchToggle(toggle.isOn);
    }

    private void OnSwitchToggle(bool value)
    {
        SoundManager.Instance.PlayButtonClickSound();

        if (value)
        {
            handleImage.rectTransform.anchoredPosition = handlePosition;
            handleImage.sprite = onSprite;
        }
        else
        {
            handleImage.rectTransform.anchoredPosition = -1.0f * handlePosition;
            handleImage.sprite = offSprite;
        }
    }
}
