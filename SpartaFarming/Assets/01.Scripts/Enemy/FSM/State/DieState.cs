using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState<T> : FSM
{
    private T Owner;
    private IDie<T> Idie;

    // »ý¼ºÀÚ
    public DieState(T owner, IDie<T> die = null)
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
