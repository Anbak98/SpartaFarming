using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingState<T> : FSM
{
    private T Owner;
    private ITraking<T> Itracking;

    // »ý¼ºÀÚ
    public TrackingState(T owner, ITraking<T> itracking = null)
    {
        this.Owner = owner;
        this.Itracking = itracking;
    }

    public override void FSM_Enter()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : Tracking Enter");

        Itracking.ITraking_Enter();
    }

    public override void FSM_Excute()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : Tracking");

        Itracking.ITraking();
    }

    public override void FSM_Exit()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : Tracking Exit");

        Itracking.ITraking_Exit();
    }
}
