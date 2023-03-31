using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private float timer;
    private float time;

    public void Enter(Bot bot)
    {
        bot.ChangeAnim(CharacterAnimator.Anim.Idle);

        timer = 0.0f;
        time = Random.Range(0.75f, 1.25f);
    }

    public void Execute(Bot bot)
    {
        timer += Time.deltaTime;
        if (timer >= time)
        {
            if (bot.HasTarget() && !bot.Attacked())
            {
                bot.ChangeState(new AttackState());
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
