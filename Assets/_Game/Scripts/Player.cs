using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Player : Character
{
    public static Player Instance { get; private set; }

    public Character Killer { get; private set; }
    public int Rank { get; private set; }
    public int Gold { get; private set; }

    private const float MIN_MOVE_DISTANCE = 0.1f;
    private const string PLAYER_PREFS_PLAYER_NAME = "PlayerName";

    private Joystick joystick;
    private Vector3 moveDirection;
    private bool thrown;
    private float throwTimer;
    private float throwTime = 0.35f;

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
    }

    public void SetPlayerName(string value)
    {
        SetName(value);

        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME, GetName());
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

        SetPlayerName(PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME, this.GetType().ToString()));
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
    }

    protected override void GetObjectColorSO(out ObjectColorSO objectColorSO)
    {
        objectColorSO = ResourceManager.Instance.GetFirstUnusedObjectColorSO();
    }

    protected override void GetSkinSetSO(out SkinSetSO skinSetSO)
    {
        skinSetSO = ResourceManager.Instance.GetSelectedSkinSetSO();
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
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, GetAgent().angularSpeed * Time.deltaTime);
        }

        if (moveDirection.magnitude >= MIN_MOVE_DISTANCE)
        {
            GetAgent().Move(moveDirection * GetAgent().speed * GetMultiplier() * Time.deltaTime);

            ChangeAnim(CharacterAnimator.Anim.Run);

            ResetAttack();
        }
    }

    private void HandleAttack()
    {
        if (moveDirection.magnitude < MIN_MOVE_DISTANCE)
        {
            if (!HasTarget())
            {
                ChangeAnim(CharacterAnimator.Anim.Idle);
            }
            else if (!Attacked())
            {
                Attack();

                throwTimer = 0.0f;
                thrown = false;
            }
        }

        if (Attacked())
        {
            throwTimer += Time.deltaTime;
            if (!thrown && throwTimer >= throwTime)
            {
                thrown = true;

                Throw();
            }
        }
    }
}
