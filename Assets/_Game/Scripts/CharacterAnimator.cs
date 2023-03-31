using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public enum Anim
    {
        Idle = 0,
        Run = 1,
        Attack = 2,
        Death = 3,
        Victory = 4,
        Dance = 5,
    }

    private Animator animator;
    private Anim anim = Anim.Idle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnim(Anim newAnim)
    {
        if (anim == newAnim)
        {
            return;
        }

        anim = newAnim;

        animator.SetTrigger(anim.ToString());
    }
}
