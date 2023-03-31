using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{
    [SerializeField]
    private bool isVisible = true;

    private void Start()
    {
        HideVirtualControls();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        joystickImage.rectTransform.position = eventData.position;

        base.OnPointerDown(eventData);

        if (isVisible)
        {
            ShowVirtualControls();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (isVisible)
        {
            HideVirtualControls();
        }
    }

    private void ShowVirtualControls()
    {
        joystickImage.gameObject.SetActive(true);
    }

    private void HideVirtualControls()
    {
        joystickImage.gameObject.SetActive(false);
    }
}
