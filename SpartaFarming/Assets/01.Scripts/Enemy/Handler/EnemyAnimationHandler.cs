using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHandler 
{
    private Animator animator;
    private Dictionary<EnemyAnimationState, Action> animationAction;

    public EnemyAnimationHandler(Animator animator) 
    {
        this.animator = animator;

        // 액션 추가 
        animationAction = new Dictionary<EnemyAnimationState, Action>();

        animationAction.Add(EnemyAnimationState.Idle, () =>
        {
            animator.SetBool("Run", false);
        });
        animationAction.Add(EnemyAnimationState.Run, () =>
        {
            animator.SetBool("Run" , true);
        });
        animationAction.Add(EnemyAnimationState.Attack, () =>
        {
            animator.SetTrigger("Attack");
        });
        animationAction.Add(EnemyAnimationState.Hit, () =>
        {
            animator.SetTrigger("Hit");
        });
        animationAction.Add(EnemyAnimationState.Die, () =>
        {
            animator.SetBool("Dead", true);    
        });
    }

    public void ChangeAnimator(EnemyAnimationState nextState) 
    {
        if(animationAction.ContainsKey(nextState))
            animationAction[nextState]?.Invoke();
    }
}
