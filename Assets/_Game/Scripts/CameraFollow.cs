using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [System.Serializable]
    public struct CameraData
    {
        public Vector3 Offset;
        public Vector3 EulerAngles;
    }

    public enum State
    {
        MainMenu = 0,
        Shop = 1,
        Gameplay = 2,
    }

    [SerializeField]
    private float lerpTime = 10.0f;
    [SerializeField]
    private CameraData mainMenuData;
    [SerializeField]
    private CameraData shopData;
    [SerializeField]
    private CameraData gameplayData;

    private Player player;
    private State state;
    private Vector3 targetOffset;
    private Vector3 offset;
    private Vector3 targetEulerAngles;
    private Vector3 eulerAngles;

    private void Start()
    {
        ChangeState(State.MainMenu);

        offset = targetOffset;
        eulerAngles = targetEulerAngles;
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            player = Player.Instance;
        }

        if (state == State.Gameplay && targetOffset != gameplayData.Offset * player.GetMultiplier())
        {
            targetOffset = gameplayData.Offset * player.GetMultiplier();
        }

        offset = Vector3.Lerp(offset, targetOffset, lerpTime * Time.deltaTime);
        transform.position = player.transform.position + offset;

        eulerAngles = Vector3.Lerp(eulerAngles, targetEulerAngles, lerpTime * Time.deltaTime);
        transform.rotation = Quaternion.Euler(eulerAngles);
    }

    public void ChangeState(State state)
    {
        this.state = state;

        switch (this.state)
        {
            case State.MainMenu:
                targetOffset = mainMenuData.Offset;
                targetEulerAngles = mainMenuData.EulerAngles;
                break;
            case State.Shop:
                targetOffset = shopData.Offset;
                targetEulerAngles = shopData.EulerAngles;
                break;
            case State.Gameplay:
                targetOffset = gameplayData.Offset;
                targetEulerAngles = gameplayData.EulerAngles;
                break;
        }
    }
}
