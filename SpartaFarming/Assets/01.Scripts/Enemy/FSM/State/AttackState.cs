using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState<T , F> : FSM
{
    private T Owner;
    private IAttack<F> Iattack;

    // »ý¼ºÀÚ
    public AttackState(T owner, IAttack<F> attack = null)
    {
        this.Owner = owner;
        this.Iattack = attack;
    }

    public override void FSM_Enter()
    {

    }

    public override void FSM_Excute()
    {

    }

    public override void FSM_Exit()
    {

    }
}
