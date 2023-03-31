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
            ResourceManager.Instance.ResetRemainingColorList();

            Player.Instance.ResetState();
        }

        LevelManager.Instance.LoadLevel();

        state = State.GameWaited;

        OnStateChanged?.Invoke(this, EventArgs.Empty);

        cameraFollow.ChangeState(CameraFollow.State.MainMenu);

        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<MainMenuCanvas>();
    }

    public void StartGame()
    {
        state = State.GameStarted;

        OnStateChanged?.Invoke(this, EventArgs.Empty);

        cameraFollow.ChangeState(CameraFollow.State.Gameplay);

        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<GameplayCanvas>();
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

    public void OpenShop()
    {
        cameraFollow.ChangeState(CameraFollow.State.Shop);

        Player.Instance.ChangeAnim(CharacterAnimator.Anim.Dance);

        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<SkinShopCanvas>();
    }

    public void CloseShop()
    {
        cameraFollow.ChangeState(CameraFollow.State.MainMenu);

        Player.Instance.ChangeAnim(CharacterAnimator.Anim.Idle);

        UIManager.Instance.CloseAll();
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

        StartCoroutine(DelayActionCoroutine(3.0f, () =>
        {
            delayAction?.Invoke();
        }));
    }

    private IEnumerator DelayActionCoroutine(float delayTime, Action action)
    {
        yield return new WaitForSeconds(delayTime);

        action?.Invoke();
    }

    private void ResetStaticData()
    {
        Bot.ResetStaticData();
    }
}