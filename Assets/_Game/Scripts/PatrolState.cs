using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private float timer;
    private float time;
    private float attackTime;
    private Vector3 direction;

    public void Enter(Bot bot)
    {
        bot.ChangeAnim(CharacterAnimator.Anim.Run);

        bot.ResetAttack();

        timer = 0.0f;
        time = Random.Range(2.0f, 4.0f);
        attackTime = Random.Range(time * 0.3f, time * 0.7f);
        direction = Utilities.GetRandomHorizontalDirection();
    }

    public void Execute(Bot bot)
    {
        timer += Time.deltaTime;
        if (timer >= time)
        {
            bot.ChangeState(new IdleState());
        }
        else
        {
            if (timer >= attackTime)
            {
                if (bot.HasTarget() && !bot.Attacked())
                {
                    bot.ChangeState(new AttackState());
                }
            }

            if (bot.HitObstacle(direction))
            {
                direction = bot.GetSuitableDirection();
            }

            bot.HandleMovement(direction);
        }
    }

    public void Exit(Bot bot)
    {

    }
}
