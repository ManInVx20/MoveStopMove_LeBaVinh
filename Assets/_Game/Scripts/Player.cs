using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Player : Character
{
    public static Player Instance { get; private set; }

    public const string PLAYER_NAME_DEFAULT = "You";

    public Character Killer { get; private set; }
    public int Rank { get; private set; }
    public int Gold { get; private set; }

    private const float MIN_MOVE_DISTANCE = 0.01f;

    private Joystick joystick;
    private Vector3 moveDirection;
    private bool thrown;
    private float throwTimer;
    private float throwTimerMax = 0.35f;
    private bool boosted;
    private float boostTimer;
    private float boostTimerMax = 1.0f;

    public override void Victory()
    {
        base.Victory();

        moveDirection = Vector3.zero;
    }

    public override void Hit(Character killer)
    {
        base.Hit(killer);

        Killer = killer;
        Rank = LevelManager.Instance.GetBotSpawner().BotRemainingCount + 1;

        GameManager.Instance.Fail();
    }

    public override void LevelUp(Character target)
    {
        base.LevelUp(target);

        Instantiate(ResourceManager.Instance.LevelUpPrefab, UIManager.Instance.GetUI<GameplayCanvas>().transform).Initialize(1);

        if (GetLevel() <= target.GetLevel())
        {
            Gold += 1;
        }
        else
        {
            Gold += 2;
        }

        Rank = LevelManager.Instance.GetBotSpawner().BotRemainingCount + 1;
        if (Rank == 1)
        {
            Victory();

            GameManager.Instance.Victory();
        }

        boosted = true;
        boostTimer = 0.0f;
    }

    public void SetPlayerName(string value)
    {
        SetName(value);

        GameDataManager.Instance.GetGameData().PlayerName = GetName();

        GameDataManager.Instance.WriteFile();
    }

    public void SetPlayerSkin(SkinSO skinSO, bool pernament = true)
    {
        SkinSO hatSkinSO = ResourceManager.Instance.GetSkinSOBySkinTypeAndSkinName(SkinType.Hat,
            GameDataManager.Instance.GetGameData().HatSkinName);
        SkinSO pantSkinSO = ResourceManager.Instance.GetSkinSOBySkinTypeAndSkinName(SkinType.Pant,
            GameDataManager.Instance.GetGameData().PantSkinName);
        SkinSO shieldSkinSO = ResourceManager.Instance.GetSkinSOBySkinTypeAndSkinName(SkinType.Accessary,
            GameDataManager.Instance.GetGameData().ShieldSkinName);
        SkinSO fullSetSkinSO = ResourceManager.Instance.GetSkinSOBySkinTypeAndSkinName(SkinType.FullSet,
            GameDataManager.Instance.GetGameData().FullSetSkinName);

        if (pernament)
        {
            UpdateSkinSO(hatSkinSO, pantSkinSO, shieldSkinSO, fullSetSkinSO);
        }
        else
        {
            switch (skinSO.SkinType)
            {
                case SkinType.Hat:
                    hatSkinSO = skinSO;
                    fullSetSkinSO = null;
                    break;
                case SkinType.Pant:
                    pantSkinSO = skinSO;
                    fullSetSkinSO = null;
                    break;
                case SkinType.Accessary:
                    shieldSkinSO = skinSO;
                    fullSetSkinSO = null;
                    break;
                case SkinType.FullSet:
                    hatSkinSO = null;
                    pantSkinSO = null;
                    shieldSkinSO = null;
                    fullSetSkinSO = skinSO;
                    break;
            }

            UpdateSkinSO(hatSkinSO, pantSkinSO, shieldSkinSO, fullSetSkinSO, false);
        }
    }

    protected override void Initialize()
    {
        Instance = this;

        base.Initialize();
    }

    protected override void Begin()
    {
        base.Begin();

        GetAgent().Warp(Vector3.zero);
        transform.localRotation = Quaternion.identity;

        SetPlayerName(GameDataManager.Instance.GetGameData().PlayerName);
        SetLevel(1);

        Killer = null;
        Rank = 50;
        Gold = 0;
    }

    protected override void Execute()
    {
        base.Execute();

        if (IsDead() || !GameManager.Instance.IsGameStarted())
        {
            return;
        }

        HandleInput();
        HandleMovement();
        HandleAttack();
        HandleBooster();
    }

    protected override void GetObjectColorSO(out ObjectColorSO objectColorSO)
    {
        objectColorSO = ResourceManager.Instance.GetFirstUnusedObjectColorSO();
    }

    protected override void GetSkinSO(out SkinSO hatSkinSO, out SkinSO pantSkinSO, out SkinSO shieldSkinSO, out SkinSO fullSetSkinSO)
    {
        hatSkinSO = ResourceManager.Instance.GetSkinSOBySkinTypeAndSkinName(SkinType.Hat,
            GameDataManager.Instance.GetGameData().HatSkinName);
        pantSkinSO = ResourceManager.Instance.GetSkinSOBySkinTypeAndSkinName(SkinType.Pant,
            GameDataManager.Instance.GetGameData().PantSkinName);
        shieldSkinSO = ResourceManager.Instance.GetSkinSOBySkinTypeAndSkinName(SkinType.Accessary,
            GameDataManager.Instance.GetGameData().ShieldSkinName);
        fullSetSkinSO = ResourceManager.Instance.GetSkinSOBySkinTypeAndSkinName(SkinType.FullSet,
            GameDataManager.Instance.GetGameData().FullSetSkinName);
    }

    protected override void GetWeaponSO(out WeaponSO weaponSO)
    {
        weaponSO = ResourceManager.Instance.GetWeaponSOByName(GameDataManager.Instance.GetGameData().WeaponName);
    }

    private void HandleInput()
    {
        moveDirection = Vector3.zero;

        if (joystick == null)
        {
            joystick = FindObjectOfType<Joystick>();
        }

        if (joystick != null)
        {
            moveDirection = new Vector3(joystick.Input.x, 0.0f, joystick.Input.y).normalized;
        }
    }

    private void HandleMovement()
    {
        if (moveDirection.sqrMagnitude >= MIN_MOVE_DISTANCE)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, GetAgent().angularSpeed * Time.deltaTime);
        }

        if (moveDirection.sqrMagnitude >= MIN_MOVE_DISTANCE)
        {
            if (boosted)
            {
                GetAgent().Move(moveDirection * GetAgent().speed * GetMultiplier() * 1.5f * Time.deltaTime);
            }
            else
            {
                GetAgent().Move(moveDirection * GetAgent().speed * GetMultiplier() * Time.deltaTime);
            }

            ChangeAnim(CharacterAnimator.Anim.Run);

            ResetAttack();
        }
    }

    private void HandleAttack()
    {
        if (moveDirection.magnitude < MIN_MOVE_DISTANCE)
        {
            if (!Attacked())
            {
                if (!HasTarget())
                {
                    ChangeAnim(CharacterAnimator.Anim.Idle);
                }
                else
                {
                    Attack();

                    throwTimer = 0.0f;
                    thrown = false;
                }
            }
        }

        if (Attacked())
        {
            throwTimer += Time.deltaTime;
            if (!thrown && throwTimer >= throwTimerMax)
            {
                thrown = true;

                Throw();
            }
        }
    }

    private void HandleBooster()
    {
        if (boosted)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer >= boostTimerMax)
            {
                boosted = false;
            }
        }
    }
}
