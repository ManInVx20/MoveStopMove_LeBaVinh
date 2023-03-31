using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt = 0,
        LookAtInverted = 1,
        CameraFoward = 2,
        CameraFowardInverted = 3,
    }

    [SerializeField]
    private Mode mode;

    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(cameraTransform);
                break;
            case Mode.LookAtInverted:
                Vector3 directionFromCamera = transform.position - cameraTransform.position;
                transform.LookAt(transform.position + directionFromCamera);
                break;
            case Mode.CameraFoward:
                transform.forward = cameraTransform.forward;
                break;
            case Mode.CameraFowardInverted:
                transform.forward = -cameraTransform.forward;
                break;
        }
    }
}
