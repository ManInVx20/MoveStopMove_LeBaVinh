using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private float timer;
    private float time;

    public void Enter(Bot bot)
    {
        bot.Attack();

        timer = 0.0f;
        time = 0.35f;
    }

    public void Execute(Bot bot)
    {
        timer += Time.deltaTime;
        if (timer >= time)
        {
            bot.Throw();

            int randomValue = Random.Range(0, 2);
            if (randomValue == 0)
            {
                bot.ChangeState(new IdleState());
            }
            else
            {
                bot.ChangeState(new PatrolState());
            }
        }
    }

    public void Exit(Bot bot)
    {

    }
}
