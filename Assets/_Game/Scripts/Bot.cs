using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bot : Character
{
    public static event EventHandler OnAnyBotDeath;
    public static event EventHandler OnAnyBotDespawned;

    public event EventHandler OnBotEnteredPlayerAttackRange;
    public event EventHandler OnBotExitedPlayerAttackRange;

    private IState state;
    private TargetIndicatorUI targetIndicatorUI;

    public static void ResetStaticData()
    {
        OnAnyBotDeath = null;
        OnAnyBotDespawned = null;
    }

    public override void ReturnToPool()
    {
        base.ReturnToPool();

        DespawnCharacterInfoUI();

        DespawnTargetIndicatorUI();
    }

    public void ChangeState(IState newState)
    {
        if (state == newState)
        {
            return;
        }

        state?.Exit(this);

        state = newState;

        state?.Enter(this);
    }

    public void HandleMovement(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, GetAgent().angularSpeed * Time.deltaTime);
        }

        GetAgent().Move(direction * GetAgent().speed * GetMultiplier() * Time.deltaTime);
    }

    public bool HitObstacle(Vector3 direction)
    {
        Vector3 origin = transform.position + Vector3.up;
        if (Physics.SphereCast(origin, 1.0f, direction, out RaycastHit hit, 2.0f))
        {
            if (hit.transform.TryGetComponent<Obstacle>(out _))
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 GetSuitableDirection()
    {
        Vector3 direction;
        do
        {
            direction = Utilities.GetRandomHorizontalDirection();
        }
        while (HitObstacle(direction));

        return direction;
    }

    public void EnterPlayerAttackRange()
    {
        OnBotEnteredPlayerAttackRange?.Invoke(this, EventArgs.Empty);
    }

    public void ExitPlayerAttackRange()
    {
        OnBotExitedPlayerAttackRange?.Invoke(this, EventArgs.Empty);
    }

    public override void Hit(Character killer)
    {
        base.Hit(killer);

        DespawnTargetIndicatorUI();

        ChangeState(null);

        OnAnyBotDeath?.Invoke(this, EventArgs.Empty);
    }

    protected override void Begin()
    {
        base.Begin();

        targetIndicatorUI = ResourceManager.Instance.TargetIndicatorUIPool.GetPrefabInstance();
        targetIndicatorUI.Initialize(this, GetNormalColor());

        SetName(ResourceManager.Instance.GetRandomName());

        int playerLevel = Player.Instance.GetLevel();
        int minLevel, maxLevel;
        if (playerLevel <= 3)
        {
            minLevel = 1;
            maxLevel = 3;
        }
        else
        {
            minLevel = Mathf.FloorToInt(playerLevel * 0.75f);
            maxLevel = Mathf.FloorToInt(playerLevel * 1.25f);
        }
        SetLevel(UnityEngine.Random.Range(minLevel, maxLevel));

        ChangeState(new IdleState());
    }

    protected override void Execute()
    {
        base.Execute();

        if (IsDead() || GameManager.Instance.IsGameWaited())
        {
            return;
        }


        state?.Execute(this);
    }

    protected override void Despawn()
    {
        base.Despawn();

        DespawnWeapon();

        ReturnToPool();

        OnAnyBotDespawned?.Invoke(this, EventArgs.Empty);
    }

    protected override void GetObjectColorSO(out ObjectColorSO objectColorSO)
    {
        objectColorSO = ResourceManager.Instance.GetRandomObjectColorSO();
    }

    protected override void GetSkinSetSO(out SkinSetSO skinSetSO)
    {
        if (UnityEngine.Random.Range(0, 3) > 0)
        {
            skinSetSO = null;
        }
        else
        {
            skinSetSO = ResourceManager.Instance.GetRandomSkinSetSO();
        }
    }

    protected override void GetWeaponSO(out WeaponSO weaponSO)
    {
        weaponSO = ResourceManager.Instance.GetRandomWeaponSO();
    }

    private void DespawnTargetIndicatorUI()
    {
        if (targetIndicatorUI != null)
        {
            targetIndicatorUI.Despawn();
            targetIndicatorUI = null;
        }
    }
}
