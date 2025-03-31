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
        //Debug.Log($"Owner : {Owner.GetType().Name} : Attack Enter");

        if(Iattack != null)
            Iattack.IAttack_Enter();
    }

    public override void FSM_Excute()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : Attack");

        if (Iattack != null)
            Iattack.IAttack();
    }

    public override void FSM_Exit()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : Attack Exit");

        if (Iattack != null)
            Iattack.IAttack_Exit();

    }
}
