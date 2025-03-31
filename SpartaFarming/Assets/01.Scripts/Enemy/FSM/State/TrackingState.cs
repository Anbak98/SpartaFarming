using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingState<T , F> : FSM
{
    private T Owner;
    private ITraking<F> Itracking;

    // »ý¼ºÀÚ
    public TrackingState(T owner, ITraking<F> itracking = null)
    {
        this.Owner = owner;
        this.Itracking = itracking;
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
