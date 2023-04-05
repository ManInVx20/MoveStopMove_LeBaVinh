using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    public class OnValueChangedEventArgs : EventArgs
    {
        public bool Value;
    }
    public event EventHandler<OnValueChangedEventArgs> OnValueChanged;

    [SerializeField]
    private GameObject onGameObject;
    [SerializeField]
    private GameObject offGameObject;

    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnSwitchToggle);
    }

    public void OnSwitchToggle(bool value)
    {
        if (value)
        {
            onGameObject.SetActive(true);
            offGameObject.SetActive(false);
        }
        else
        {
            onGameObject.SetActive(false);
            offGameObject.SetActive(true);
        }

        OnValueChanged?.Invoke(this, new OnValueChangedEventArgs
        {
            Value = value
        });
    }
}
