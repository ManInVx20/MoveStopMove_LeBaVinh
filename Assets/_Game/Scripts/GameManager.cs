using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class GameManager : Singleton<GameManager>
{
    public event EventHandler OnStateChanged;

    [SerializeField]
    private CameraFollow cameraFollow;
    [SerializeField]
    private Camera UICamera;

    private enum State
    {
        GameWaited = 0,
        GameStarted = 1,
        GameFinished = 2,
    }
    private State state;

    private void Awake()
    {
        ResetStaticData();
    }

    private void Start()
    {
        WaitGame(true);
    }

    public bool IsGameWaited()
    {
        return state == State.GameWaited;
    }

    public bool IsGameStarted()
    {
        return state == State.GameStarted;
    }

    public bool IsGameFinished()
    {
        return state == State.GameFinished;
    }

    public void WaitGame(bool firstTime = false)
    {
        if (!firstTime)
        {
            ResourceManager.Instance.ResetState();

            Player.Instance.ResetState();
        }

        LevelManager.Instance.LoadLevel();

        state = State.GameWaited;

        OnStateChanged?.Invoke(this, EventArgs.Empty);

        cameraFollow.ChangeState(CameraFollow.State.MainMenu);

        UICamera.gameObject.SetActive(true);

        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<MainMenuCanvas>();
        UIManager.Instance.OpenUI<CurrencyCanvas>();
    }

    public void StartGame()
    {
        state = State.GameStarted;

        OnStateChanged?.Invoke(this, EventArgs.Empty);

        cameraFollow.ChangeState(CameraFollow.State.Gameplay);

        UICamera.gameObject.SetActive(false);

        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<GameplayCanvas>();
        UIManager.Instance.OpenUI<TopBarCanvas>();
        UIManager.Instance.OpenUI<ControlCanvas>();
    }

    public void Fail()
    {
        FinishGame(() =>
        {
            UIManager.Instance.OpenUI<FailCanvas>();

            SoundManager.Instance.PlayFailSound();
        });
    }

    public void Victory()
    {
        FinishGame(() =>
        {
            UIManager.Instance.OpenUI<VictoryCanvas>();

            LevelManager.Instance.NextLevel();

            SoundManager.Instance.PlayVictorySound();
        });
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }

    public void OpenSkinShop()
    {
        cameraFollow.ChangeState(CameraFollow.State.Shop);

        Player.Instance.ChangeAnim(CharacterAnimator.Anim.Dance);

        UIManager.Instance.CloseUI<MainMenuCanvas>();
        UIManager.Instance.OpenUI<SkinShopCanvas>();
    }

    public void CloseSkinShop()
    {
        cameraFollow.ChangeState(CameraFollow.State.MainMenu);

        Player.Instance.SetPlayerSkin(null);
        Player.Instance.ChangeAnim(CharacterAnimator.Anim.Idle);

        UIManager.Instance.CloseUI<SkinShopCanvas>();
        UIManager.Instance.OpenUI<MainMenuCanvas>();
    }

    public void OpenWeaponShop()
    {
        cameraFollow.ChangeState(CameraFollow.State.Shop);

        Player.Instance.ChangeAnim(CharacterAnimator.Anim.Dance);

        UIManager.Instance.CloseUI<MainMenuCanvas>();
        UIManager.Instance.OpenUI<WeaponShopCanvas>();
    }

    public void CloseWeaponShop()
    {
        cameraFollow.ChangeState(CameraFollow.State.MainMenu);

        Player.Instance.ChangeAnim(CharacterAnimator.Anim.Idle);

        UIManager.Instance.CloseUI<WeaponShopCanvas>();
        UIManager.Instance.OpenUI<MainMenuCanvas>();
    }

    private void FinishGame(Action delayAction)
    {
        if (IsGameFinished())
        {
            return;
        }

        state = State.GameFinished;

        OnStateChanged?.Invoke(this, EventArgs.Empty);

        ResourceManager.Instance.IncreaseGoldAmount(Player.Instance.Gold);

        UIManager.Instance.CloseAll();

        StartCoroutine(Utilities.DelayActionCoroutine(3.0f, () =>
        {
            delayAction?.Invoke();
        }));
    }

    private void ResetStaticData()
    {
        Bot.ResetStaticData();
    }
}
