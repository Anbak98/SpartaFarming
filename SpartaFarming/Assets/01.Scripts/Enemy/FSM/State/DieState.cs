using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState<T , F> : FSM
{
    private T Owner;
    private IDie<F> Idie;

    // »ý¼ºÀÚ
    public DieState(T owner, IDie<F> die = null)
    {
        this.Owner = owner;
        this.Idie = die;
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
