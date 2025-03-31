using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState<T> : FSM
{
    private T Owner;
    private IAttack<T> Iattack;

    // »ý¼ºÀÚ
    public AttackState(T owner, IAttack<T> attack = null)
    {
        this.Owner = owner;
        this.Iattack = attack;
    }

    public override void FSM_Enter()
    {
        Debug.Log($"Owner : {Owner.GetType().Name} : Attack Enter");

    }

    public override void FSM_Excute()
    {
        Debug.Log($"Owner : {Owner.GetType().Name} : Attack");

    }

    public override void FSM_Exit()
    {
        Debug.Log($"Owner : {Owner.GetType().Name} : Attack Exit");

    }
}
