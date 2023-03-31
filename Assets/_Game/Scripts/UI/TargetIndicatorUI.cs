using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicatorUI : PoolableObject
{
    [SerializeField]
    private Indicator indicator;

    private Bot owner;
    private Player player;
    private Camera cam;
    private Vector3 centerScreenPosition;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        cam = Camera.main;
        centerScreenPosition = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
    }

    private void LateUpdate()
    {
        Vector3 directionFromTargetToPlayer = owner.transform.position - player.transform.position;
        Ray ray = new Ray(player.transform.position, directionFromTargetToPlayer);

        // [0] = Left, [1] = Right, [2] = Down, [3] = Up
        float minDistance = Mathf.Infinity;
        Plane[] planeArray = GeometryUtility.CalculateFrustumPlanes(cam);
        for (int i = 0; i < planeArray.Length; i++)
        {
            if (planeArray[i].Raycast(ray, out float distance))
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
        }
        minDistance = Mathf.Clamp(minDistance, 0.0f, directionFromTargetToPlayer.magnitude);

        if (directionFromTargetToPlayer.magnitude > minDistance)
        {
            indicator.Show();
        }
        else
        {
            indicator.Hide();
        }

        Vector3 worldPosition = ray.GetPoint(minDistance);
        Vector3 screenPosition = cam.WorldToScreenPoint(worldPosition);
        screenPosition.z = 0.0f;
        indicator.SetScreenPosition(screenPosition);
        
        Vector3 directionFromCenterScreen = (screenPosition - centerScreenPosition).normalized;
        float angle = Vector3.SignedAngle(directionFromCenterScreen, Vector3.up, -Vector3.forward);
        indicator.SetArrowRotation(angle);
    }

    public void Initialize(Bot owner, Color color)
    {
        transform.SetParent(UIManager.Instance.GetUI<GameplayCanvas>().transform);

        indicator.SetColor(color);

        this.owner = owner;
        this.owner.OnCharacterInfoChanged += Character_OnCharacterInfoChanged;
    }

    public void Despawn()
    {
        ReturnToPool();

        owner = null;
    }

    private void Character_OnCharacterInfoChanged(object sender, Character.OnCharacterInfoChangedArgs args)
    {
        indicator.SetLevelText(args.Level);
    }
}
